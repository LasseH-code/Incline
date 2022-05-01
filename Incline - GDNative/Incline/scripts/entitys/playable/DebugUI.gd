extends Control

onready var fps = $TopL/FPS
onready var p_vel = $"TopL/Player Velocity"

func _process(delta):
	fps.text = String(round(1.0/delta)) + " FPS"


func _on_Player_debug_info(velocity):
	p_vel.text = String(velocity) 
