extends Path3D

@export var waves : Array[Wave]
@export var rest_time_between_waves : float = 5.0

@onready var spawn_location : PathFollow3D = $PTF_Locations

@onready var spawn_timer : Timer = $TMR_SpawnTime
@onready var rest_timer : Timer = $TMR_RestTime

signal wave_start
signal wave_end

var wave_counter = 0
var spawn_counter = 0
var current_wave : Wave

var wave_process = false


func _ready() -> void:
	wave_start.connect(_on_wave_started)
	wave_end.connect(_on_wave_ended)
	rest_timer.wait_time = rest_time_between_waves


func _process(delta) -> void:
	if wave_process :
		if spawn_counter <= 0:
			emit_signal("wave_end")

func start() -> void:
	start_wave()


func start_wave() -> void:
	wave_counter += 1
	var wave : Wave
	for w in waves :
		if w.number_wave == wave_counter :
			wave = waves[wave_counter - 1]
	if wave == null :
		print("end of waves")
		return
	
	current_wave = wave
	spawn_counter = wave.max_spawn_number
	spawn_timer.wait_time = wave.interval_of_spawn
	wave_start.emit()



func end_wave() -> void:
	wave_end.emit()


func _on_wave_started() -> void:
	spawn_timer.start()
	wave_process = true


func _on_wave_ended() -> void:
	spawn_timer.stop()
	wave_process = false
	rest_timer.start()
	print("wave_ended")


func _on_tmr_spawn_time_timeout() -> void:
	spawn(current_wave.size_of_group)


func spawn(groupe_size : int = 1) -> void:
	for i in groupe_size:
		var entity = waves[0].monster_type.instantiate()
		spawn_location.progress_ratio = randf()
		entity.position = spawn_location.position
		add_child(entity)
		
		spawn_counter -= 1


func _on_tmr_rest_time_timeout() -> void:
	start_wave()
