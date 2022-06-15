extends Spatial

export(NodePath) var player_path
onready var player = get_node(player_path)
export(NodePath) var nav_path
onready var nav = get_node(nav_path)

export(PackedScene) var probe_sc:PackedScene

export(Array, NodePath) var spawn_areas:Array

export(int) var initial_spawns = 0

func _ready():
	spawn_enemys_random(initial_spawns)

func spawn_enemys_random(num:int) -> void:
	for i in range(num):
		spawn_enemy_random()

func spawn_enemy_random() -> void:
	var a:Area = get_node(spawn_areas[randi()%spawn_areas.size()])
	var c:CollisionShape = a.get_child(0)
	var s:BoxShape = c.shape
	var pos:Vector3 = Vector3(fmod(randf(),s.extents.x)-(s.extents.x/2),fmod(randf(),s.extents.y)-(s.extents.y/2),0)
	print(c.global_transform.origin+pos)
	spawn_enemy(a.global_transform.origin+pos)

func spawn_enemy(position:Vector3) -> void:
	var t = probe_sc.instance()
	add_child(t)
	t.global_transform.origin = position
	t.drop()
