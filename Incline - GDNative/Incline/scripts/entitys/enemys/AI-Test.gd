extends "res://scripts/entitys/MovableEntity.gd"

enum {
	idle,
	alert,
	following,
	returning,
	waiting,
	swarm
}

var ind

var state = idle
var statetemp
var statechange = true
var target
var path = []
var path_node = 0
var player
var base = Vector3()

var position
var position_player

const turn_speed = 2

export(int) var Sight_deg = 90
export(float) var speed = 20

onready var timer = $Timer
onready var timer2 = $Timer2
#onready var raycast = $head/RayCast
onready var eyes = $Eyes
onready var tracker = $Tracker
onready var trackerRC = $TrackerRC #$Tracker/RayCast
onready var sightL = $Sightrange/SightLeft
onready var sightR = $Sightrange/SightRight
onready var nav = get_parent()
onready var gun = $Spatial/ViewModel
onready var map = get_parent().get_parent()

func _ready():
	sightL.rotate_y(deg2rad(Sight_deg-1))
	sightR.rotate_y(deg2rad(-Sight_deg+1))
	#print(base.global_transform.origin)
	print(global_transform.origin)
	base = global_transform.origin

func move_to(target:Vector3):
	path = nav.get_simple_path(global_transform.origin, target)
	path_node = 0

func _on_Sightrange_body_entered(body):
	if body.is_in_group("Playable") :
		if player == null:
			player = body
		state = following
		statechange = true
		target = body
		eyes.look_at(target.global_transform.origin, Vector3.UP)
		if eyes.rotation_degrees.y > Sight_deg || eyes.rotation_degrees.y < -Sight_deg:
			state = idle
			statechange = true
		else:
			move_to(player.global_transform.origin)
		
		



func _on_Sightrange_body_exited(body):
	pass
	#state = idle

func _physics_process(delta):
	if path_node < path.size():
		var direction = (path[path_node] - global_transform.origin)
		if direction.length() < 1:
			path_node += 1
		else:
			move_and_slide(direction.normalized() * speed, Vector3.UP)
	if Input.is_action_pressed("primary"):
		gun.ShootBullet()

func _process(delta):
	if player != null:
		tracker.look_at(player.global_transform.origin, Vector3.UP)
		#var temp = translation.distance_to(player.translation)
		var temp2 = Vector3(trackerRC.global_transform.origin.x-player.global_transform.origin.x, player.global_transform.origin.y, -trackerRC.global_transform.origin.z-player.global_transform.origin.z)
		var temp3 = player.translation - global_transform.origin
		trackerRC.cast_to = Vector3(temp3.x, temp3.y, temp3.z).rotated(Vector3(0,1,0), deg2rad(-rotation_degrees.y)
		)
	if !trackerRC.is_colliding() && player != null:
		pass
		#trackerRC.cast_to = Vector3(0, 0, -trackerRC.global_transform.origin.distance_to(player.global_transform.origin))
		#trackerRC.cast_to = Vector3(0, 0, trackerRC.cast_to.z - 10)
	
	if sightL.is_colliding():
		if sightL.get_collider().is_in_group("Playable"):
			state = following
			statechange = true
			move_to(player.global_transform.origin)
	
	if sightR.is_colliding():
		if sightR.get_collider().is_in_group("Playable"):
			state = following
			statechange = true
			move_to(player.global_transform.origin)
	match state:
		#idle:
			
		alert:
			eyes.look_at(target.global_transform.origin, Vector3.UP)
			rotate_y(deg2rad(eyes.rotation.y * turn_speed))
		following:
			if !trackerRC.is_colliding():
				if timer2.time_left == 0:
					timer2.start(2)
			elif !trackerRC.get_collider().is_in_group("Playable"):
				if timer2.time_left == 0:
					timer2.start(2)
			else:
				eyes.look_at(target.global_transform.origin, Vector3.UP)
				rotate_y(deg2rad(eyes.rotation.y * turn_speed))
				if timer.time_left == 0:
					timer.start(0.1)
		waiting:
			if timer.is_stopped():
				timer.start(3)
		swarm:
			if !trackerRC.is_colliding():
				if timer2.time_left == 0:
					timer2.start(2)
			elif !trackerRC.get_collider().is_in_group("Playable"):
				if timer2.time_left == 0:
					timer2.start(2)
			else:
				eyes.look_at(target.global_transform.origin, Vector3.UP)
				rotate_y(deg2rad(eyes.rotation.y * turn_speed))
				if timer.time_left == 0:
					timer.start(0.1)
	
	if statechange:
		match state:
			idle:
				print("idle")
			following:
				print("following")
			waiting:
				print("waiting")
			returning:
				print("returning")
			swarm:
				print("swarm")
		statechange = false

func swarm(var ply):
	target = ply
	state = swarm
	statechange = true

func stop_swarm():
	state = returning
	statechange = true

func _on_Timer_timeout():
	match state:
		idle:
			print("go")
			move_to(player.global_transform.origin)
			state = following
			statechange = true
		following:
			move_to(player.global_transform.origin)
		waiting:
			move_to(base)
			state = returning
			statechange = true
		swarm:
			move_to(player.global_transform.origin)


func _on_Timer2_timeout():
	match state:
		following:
			if !trackerRC.is_colliding() || !trackerRC.get_collider().is_in_group("Playable"):
				timer.stop()
				state = waiting
				move_to(global_transform.origin)
				statechange = true
		swarm:
			if !trackerRC.is_colliding() || !trackerRC.get_collider().is_in_group("Playable"):
				map.report_lost(ind)
