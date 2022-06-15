extends KinematicBody

var path = []
var path_node = 0

var speed = 10

#export(NodePath) var nav_path
onready var nav:Navigation
#export(NodePath) var player_path
onready var player:KinematicBody
onready var setup:bool = false

func _ready():
	pass

func setup():
	nav = get_parent().nav
	player = get_parent().player
	$Timer.autostart = true
	setup = true

func _physics_process(delta) -> void:
	if setup and path_node < path.size():
		var direction:Vector3 = (path[path_node] - global_transform.origin)
		if direction.length() < 1:
			path_node+=1
		else:
			move_and_slide(direction.normalized() * speed, Vector3.UP)

func move_to(target_pos):
	path = nav.get_simple_path(global_transform.origin, target_pos)
	path_node = 0

func _on_Timer_timeout():
	move_to(player.global_transform.origin)
