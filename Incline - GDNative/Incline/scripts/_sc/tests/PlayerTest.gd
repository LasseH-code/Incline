extends Spatial

var tcounter = 0
var enemys = []
var enemys_sight_lost = []

export(PackedScene) var enemy

onready var player = $Player
onready var nav = $Navigation
onready var navmesh = $Navigation/NavigationMeshInstance
onready var timer = $Timer
onready var spawner = $Spawner


func _ready():
	randomize()
	timer.start(20)

func _process(delta):
	if Input.is_action_just_pressed("secondary"):
		print(check_for_Player(Vector3(0, 0, 0)))
		spawn_Enemy()
	if Input.is_action_just_pressed("ui_down"):
		print(randi() % 60)

func check_for_Player(pos:Vector3, rad:int = 10) -> bool:
	spawner.global_transform.origin = pos
	spawner.get_child(0).shape.radius = rad
	var bodies = spawner.get_overlapping_bodies()
	var temp = false
	for i in bodies:
		if i.is_in_group("Playable"):
			temp = true
			break
	return temp

func spawn_Enemy():
	var player_pos = player.global_transform.origin
	spawner.global_transform.origin = player_pos
	spawner.get_child(0).shape.radius = 30
	var enm_inst = enemy.instance(PackedScene.GEN_EDIT_STATE_INSTANCE)
	get_tree().root.get_child(4).get_child(6).add_child(enm_inst)
	var pos = Vector3(player_pos.x + randi() % 60 - 30, player_pos.y, player_pos.z + randi() % 60 - 30)
	enm_inst.global_transform.origin = pos
	enm_inst.ind = enemys.size()
	enemys.append(enm_inst)

func report_Player(var re_player):
	pass

func report_lost(var ind):
	pass

func _on_Timer_timeout():
	tcounter += 1
	

func _on_Spawner_body_entered(body):
	pass
