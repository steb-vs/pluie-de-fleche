extends CharacterBody3D

signal damage_city(damage : int)

@onready var model_head = $CSG_Head
@onready var model_body = $CSG_Body

@onready var hurt_box_body = $A3D_HurtBoxBody
@onready var hurt_box_head = $A3D_HurtBoxHead

var movement_speed : float = 5.0
@export var distance : float
var movement_target_position : Vector3
@export var damage : int = 5

@onready var nav_agent : NavigationAgent3D = $NVA

@export var hit_points : int = 10:
	get :
		return hit_points
	set(value) :
		hit_points = value if value > 0 else 0
		
		if hit_points == 0:
			death()


# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")


func _ready():
	damage_city.connect(EventBus._on_malemoniak_damage_city)
	
	random_speed()
	
	nav_agent.path_desired_distance = 1
	nav_agent.target_desired_distance = 1
	movement_target_position = Vector3(global_position.x, 0, distance)
	call_deferred("actor_setup")


func actor_setup() -> void:
	await get_tree().physics_frame
	
	set_movement_target(movement_target_position)


func set_movement_target(movement_target: Vector3):
	nav_agent.set_target_position(movement_target)


func _physics_process(delta):
	#Navigation
	if nav_agent.is_navigation_finished():
		return
	
	var current_agent_position: Vector3 = global_position
	var next_path_position: Vector3 = nav_agent.get_next_path_position()
	
	var new_velocity: Vector3 = next_path_position - current_agent_position
	new_velocity = new_velocity.normalized()
	new_velocity = new_velocity * movement_speed
	
	velocity = new_velocity
	
		# Add the gravity.
	if not is_on_floor():
		velocity.y -= gravity * delta
	
	move_and_slide()


func take_damage(amount : int) -> void:
	hit_points -= amount
	print(hit_points)


func death() -> void:
	hurt_box_body.monitorable = false
	hurt_box_head.monitorable = false
	
	var tween = get_tree().create_tween()
	var duration : float = 1.0
	tween.tween_method(_set_shader_dissolve_value, 0.0, 1.0, duration)
	
	await tween.finished
	queue_free()


func attack_city() -> void:
	damage_city.emit(damage)
	death()

func random_speed():
	movement_speed = randf_range(movement_speed - 2, movement_speed + 2)


func _set_shader_dissolve_value(value):
	model_body.material.set_shader_parameter("dissolve_amount", value)
	model_head.material.set_shader_parameter("dissolve_amount", value)


func _on_nva_navigation_finished():
	attack_city()
