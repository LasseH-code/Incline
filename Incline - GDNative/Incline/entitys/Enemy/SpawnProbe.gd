extends Spatial

onready var down_ray:RayCast = $RayCast
onready var container:Position3D = $Indicator

func drop():
	#var pos 
	if down_ray.is_colliding():
		global_transform.origin = down_ray.get_collision_point()
		#pos = down_ray.get_collision_point()
		
	down_ray.queue_free()
	container.queue_free()
	
	var cs = get_children()
	for c in cs:
		#c.global_transform.origin = pos
		var pos = global_transform.origin
		remove_child(c)
		get_parent().add_child(c)
		global_transform.origin = pos
		c.visible = true
		if c.has_method("setup"):
			c.setup()
			print("setup called")
	queue_free()
