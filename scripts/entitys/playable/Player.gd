extends "res://scripts/entitys/MovableEntity.gd"

onready var cam_holder = $CameraHolder
onready var cam = $CameraHolder/Camera
onready var items = $Items

signal debug_info(velocity)

export(float) var max_speed = 30.0
var internal_max_speed = max_speed
export(float) var sprint_multiplier = 2.0
export(bool) var sprint_increase_max_speed = true
export(bool) var set_sprint_speed = false
export(float) var sprint_speed = max_speed/sprint_multiplier
export(bool) var set_sprint_accel = false
export(float) onready var sprint_accel = acceleration/sneak_multiplier
export(float) var sneak_multiplier = 0.3
export(bool) var sneak_decrease_max_speed = true
export(bool) var set_sneak_speed = false
export(float) var sneak_speed = max_speed/sneak_multiplier
export(bool) var set_sneak_accel = false
export(float) onready var sneak_accel = acceleration/sneak_multiplier
export(bool) var sneak_decrease_jump = true
export(float) var sneak_jump_multiplier = 0.5
export(bool) var set_sneak_jump = false
export(float) onready var sneak_jump = jump_height/sneak_multiplier
export(float) var friction = 0.05
export(float, 0, 1) var air_friction_multiplier = 0.05
export(float, 0, 1, 0.01) var friction_reduction_whilst_accel = 0
export(bool) var do_wall_friction = true
export(float) var wall_friction = 0.2
export(float) var jump_height = 50.0
export(float) var weight = 1.0
export(float) var acceleration = 20
var internal_accel = acceleration
export(float) var acceleration_rate = .1
export(Curve) var acceleration_curve
export(bool) var use_accel_as_friction = false
export(bool) var instant_turn = true
export(bool) var keep_accel_at_turn = false # Overrides instant_turn
export(bool) var abrupt_turn = true
export(float) var abrupt_turn_time = 0.2
var abrupt_turn_timer

export(float) var mouse_sensitivity = 0.1
var cam_rot = 0

var action_flags = [false, false, false, false] # up | down | left | right
var primitive_action_flags = [false, false]

func recieve_item(type:String, payloads) -> void:
	print("Recieved " + type + " item!")
	for p in payloads:
		p.get_parent().remove_child(p)
		items.add_child(p)
	if !(items.items.has(type)):
		items.items[type] = 1
	else:
		items.items[type] += 1
	items.print_items()
	items.refresh_payloads()

func create_abrupt_turn_timer() -> void:
	abrupt_turn_timer = Timer.new()
	abrupt_turn_timer.name = "att"
	abrupt_turn_timer.one_shot = true
	abrupt_turn_timer.autostart = false
	abrupt_turn_timer.wait_time = abrupt_turn_time
	add_child(abrupt_turn_timer)
	abrupt_turn_timer = $att

func _ready():
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	if use_accel_as_friction:
		friction = acceleration
	create_abrupt_turn_timer()

func take_dir_input() -> void: 
	action_flags = [false, false, false, false]
	var cam_holder_basis = cam_holder.get_global_transform().basis
	dir = Vector3.ZERO
	if Input.is_action_pressed("move_fw"):
		action_flags[0] = true
		dir -= cam_holder_basis.z
		#dir = items.forwards_m(dir)
	if Input.is_action_pressed("move_bw"):
		action_flags[1] = true
		dir += cam_holder_basis.z 
		#dir = items.backwards_m(dir)
	if Input.is_action_pressed("move_le"):
		action_flags[2] = true
		dir -= cam_holder_basis.x
		#dir = items.left_m(dir)
	if Input.is_action_pressed("move_ri"):
		action_flags[3] = true
		dir += cam_holder_basis.x 
		#dir = items.right_m(dir)
	
	if Input.is_action_just_pressed("move_jump"):
		dir.y = 1 
		#dir = items.jump_m(dir)
	
	internal_accel = acceleration
	internal_max_speed = max_speed
	
	if Input.is_action_pressed("move_sneak"):
		dir *= Vector3(sneak_multiplier, sneak_jump_multiplier, sneak_multiplier)
		if sneak_decrease_max_speed:
			internal_max_speed = max_speed*sneak_multiplier
		if set_sneak_speed:
			internal_max_speed = sneak_speed
		if set_sneak_accel:
			internal_accel = sneak_accel
		if set_sneak_jump:
			dir.y = (dir.y/dir.y) * sneak_jump
		elif sneak_decrease_jump:
			dir.y *= sneak_jump_multiplier
		#dir = items.sneak_m(dir)
	elif Input.is_action_pressed("move_sprint"):
		dir *= Vector3(sprint_multiplier, 1, sprint_multiplier)
		if sprint_increase_max_speed:
			internal_max_speed = max_speed*sprint_multiplier
		if set_sprint_speed:
			internal_max_speed = sprint_speed
		if set_sprint_accel:
			internal_accel = sprint_accel
		#dir *= Vector3(sprint_multiplier, 1, sprint_multiplier)
		#dir = items.sprint_m(dir)
	
	#dir = items.dir_m(dir)

func calc_vel(delta) -> void:
	#vel = vel.normalized()
	#vel.x = max(0, vel.x-friction)
	#vel.z = max(0, vel.z-friction)
	var interpolation_vector = Vector3(0, vel.y, 0)
	if dir.x != 0:
		interpolation_vector.x = vel.x * friction_reduction_whilst_accel
	if dir.z != 0:
		interpolation_vector.z = vel.z * friction_reduction_whilst_accel
	var friction_temp = friction
	if is_on_wall() and do_wall_friction:
		friction_temp+=wall_friction;
	if !is_on_floor():
		friction_temp *= air_friction_multiplier
	vel = vel.linear_interpolate(interpolation_vector, friction)
	
	if keep_accel_at_turn:
		if (vel.z > 0 && dir.z < 0) || (vel.z < 0 && dir.z > 0):
			vel.z = -vel.z
		if (vel.x > 0 && dir.x < 0) || (vel.x < 0 && dir.x > 0):
			vel.x = -vel.x
	elif abrupt_turn:
		pass
	elif instant_turn && ((vel.z > 0 && dir.z < 0) || (vel.z < 0 && dir.z > 0) || (vel.x > 0 && dir.x < 0) || (vel.x < 0 && dir.x > 0)):
		vel = Vector3(0, vel.y, 0)
	
	# ----- FIX THIS ---------
	if vel.z > 0:
		vel.z = min(vel.z + (dir.z * internal_accel * delta), internal_max_speed)
	else:
		vel.z = max(vel.z + (dir.z * internal_accel * delta), -internal_max_speed)
	
	if vel.x > 0:
		vel.x = min(vel.x + (dir.x * internal_accel * delta), internal_max_speed)
	else:
		vel.x = max(vel.x + (dir.x * internal_accel * delta), -internal_max_speed)
	# ------------------------
	
	vel.y -= gravity * delta
	if (is_on_floor() && dir.y > 0): #or items.jump_anyways():
		vel.y = (dir.y*jump_height) * delta
	#vel = items.vel_m(dir)

func _input(event):
	if event is InputEventMouseMotion:
		cam_holder.rotate_y(deg2rad(-event.relative.x * mouse_sensitivity))
		var cam_rot_delta = event.relative.y * mouse_sensitivity
		if cam_rot + cam_rot_delta > -90 and cam_rot + cam_rot_delta < 90:
			cam.rotate_x(deg2rad(-cam_rot_delta))
			cam_rot += cam_rot_delta
	if Input.is_action_pressed("ui_cancel"):
		get_tree().quit()

func _physics_process(delta):
	take_dir_input()
	calc_vel(delta)
	vel = move_and_slide(vel, Vector3.UP)

func _process(delta):
	emit_signal("debug_info", vel)
