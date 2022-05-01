extends Spatial

onready var barrel_end = $BarrelEnd
onready var shooting_particles = $Particles/Shooting
onready var idle_paricles = $Particles/Idle
onready var bullet_timer = $BulletTimer
onready var ray:RayCast = $"../../../GunCam/Hit Ray"

enum FireModes {
	PHYSICAL = 0,
	HITSCAN = 1
}

enum Buttons {
	PRIMARY = 0,
	SECONDARY = 1
}
var button_map = [
	"primary",
	"secondary"
]


export(String) var gun_name = "NULL"

export(PackedScene) onready var bullet
export(bool) var use_pewd

export(float) var shooting_speed = 5.0
export(float) var damage_mul = 1.0
export(float) var range_mul = 1.0
export(float) var speed_mul = 1.0
export(bool) var custom_action
export(Script) var c_action_script
export(Buttons) var button
export(FireModes) var fire_mode
export(bool) var procedual_anim = true
export(float) var p_knockback_str = 0.1
export(float) var p_idle_str = 1.0
export(NodePath) var shoot_anim
export(NodePath) var idle_anim
export(bool) var custom_anim
export(Array, NodePath) var shoot_particles
export(Array, NodePath) var idle_particles
export(Array, NodePath) var shoot_audio
export(Array, NodePath) var idle_audio
export(bool) var disable_idle_audio_on_shoot = false

var starting_pos = self.translation
var k_pos = starting_pos+Vector3(0,0,p_knockback_str)

#var pewd_bullet_id

func _ready():
	#if use_pewd:
	#	pewd_bullet_id = BulletServer.AddType("ProtoBullet", )
	bullet_timer.wait_time = 1.0/shooting_speed

func p_knockback():
	self.translation = k_pos

func spawn_bullet():
	if bullet_timer.time_left == 0:
		for p in shoot_particles:
			var temp = get_node(p)
			temp.emitting = true
		for a in shoot_audio:
			var temp = get_node(a)
			temp.stop()
			temp.play()
		if disable_idle_audio_on_shoot:
			for a in idle_audio:
				var temp = get_node(a)
				temp.playing = false
		#print("fired!")
		var temp:Spatial
		if !use_pewd:
			temp = bullet.instance()
			temp.global_transform = barrel_end.global_transform
			temp.damage *= damage_mul
			temp.distance *= range_mul
			temp.speed *= speed_mul 
		else:
			temp = bullet.instance()
			temp.global_transform = barrel_end.global_transform
			#temp.rotate_y(90);
			#temp = Pewd.create_bullet(global_transform)
		get_tree().root.add_child(temp)
		if ray.is_colliding():
			temp.look_at(ray.get_collision_point(), Vector3.UP)
			#temp.rotate(temp.global_transform.basis.x, deg2rad(-90))
		if procedual_anim:
			p_knockback()
		bullet_timer.start()
		if disable_idle_audio_on_shoot:
			for a in idle_audio:
				var tempa = get_node(a)
				tempa.playing = true

func _process(delta):
	#print("process")
	if custom_action != true:
		if Input.is_action_pressed("primary"):
			if fire_mode == FireModes.PHYSICAL:
				spawn_bullet()
			elif fire_mode == FireModes.HITSCAN:
				pass
	else:
		c_action_script.action(self)
	
	if procedual_anim:
		self.translation = self.translation.linear_interpolate(starting_pos, 0.7*p_knockback_str)
