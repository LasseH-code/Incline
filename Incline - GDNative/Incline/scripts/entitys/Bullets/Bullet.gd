extends Spatial

onready var holder = $Holder
#onready var coll_particles:Spatial = $Holder/Particles/Collision
onready var area = $Holder/Area
onready var timer = $Timer

enum KillCrit {
	NONE,
	DIST,
	TIME
}

export(float) var damage := 1.0
export(KillCrit) var criteria_type := KillCrit.DIST
export(float) var distance := 20.0
export(float) var speed := 15.0
export(float) var despawn_timer := 1.0
export(Array, NodePath) var hit_particles
export(Array, NodePath) var idle_particles
#export(Array, NodePath) var idle_particles
export(Array, NodePath) var hit_audio # UNTESTED
export(Array, NodePath) var idle_audio # UNTESTED
export(bool) var do_custom_impact # UNTESTED
export(Script) var custom_impact # UNTESTED

onready var vel = Vector3(0,0,-speed)
var moving = true
var criteria
onready var d_sq = distance*distance

func test_dist_crit() -> void:
	match criteria_type:
		KillCrit.DIST:
			#var temp = self.translation.distance_squared_to(criteria)
			if holder.translation.distance_squared_to(criteria) > d_sq:
				kill()
			continue
		KillCrit.TIME:
			if criteria+distance < OS.get_unix_time():
				kill()
			continue

func _physics_process(delta) -> void:
	#pass
	if moving:
		holder.translate(vel*delta)
	test_dist_crit()

func _on_Area_body_entered(body) -> void:
	if !do_custom_impact:
		kill()
	else:
		custom_impact.action(self)

func _ready() -> void:
	match criteria_type:
		KillCrit.DIST:
			criteria = holder.translation
			continue
		KillCrit.TIME:
			criteria = OS.get_unix_time()
			continue

func kill() -> void:
	if moving:
		moving = false
		area.visible = false
		for p in hit_particles:
			var temp = get_node(p)
			temp.emitting = true
		for p in idle_particles:
			var temp = get_node(p)
			temp.emitting = false
		for a in hit_audio:
			var temp = get_node(a)
			temp.play()
		for a in idle_audio:
			var temp = get_node(a)
			temp.stop()
		#coll_particles.owner = get_tree().root
		
		timer.start(despawn_timer)
		#queue_free()

func _on_Timer_timeout() -> void:
	queue_free()
