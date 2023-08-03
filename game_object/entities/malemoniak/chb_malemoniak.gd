extends CharacterBody3D

@onready var model_head = $CSG_Head
@onready var model_body = $CSG_Body

@onready var hurt_box_body = $A3D_HurtBoxBody
@onready var hurt_box_head = $A3D_HurtBoxHead

@export var hit_points : int = 10:
	get :
		return hit_points
	set(value) :
		hit_points = value if value > 0 else 0
		
		if hit_points == 0:
			death()


const SPEED = 5.0
const JUMP_VELOCITY = 4.5

# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")


func _physics_process(delta):
	# Add the gravity.
	if not is_on_floor():
		velocity.y -= gravity * delta
	
	move_and_slide()


func taking_damage(amount : int) -> void:
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

func _set_shader_dissolve_value(value):
	model_body.material.set_shader_parameter("dissolve_amount", value)
