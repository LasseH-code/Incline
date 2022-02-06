extends Spatial

var items = {}

func print_items():
	var i:int = 0
	for item in items:
		for j in items[item]:
			print(str(i)+": "+str(item))
			i+=1

func _ready():
	pass

func forwards_m(dir:Vector3) -> Vector3:
	return dir
func backwards_m(dir:Vector3) -> Vector3:
	return dir
func left_m(dir:Vector3) -> Vector3:
	return dir
func right_m(dir:Vector3) -> Vector3:
	return dir
func jump_m(dir:Vector3) -> Vector3:
	return dir
func shift_m(dir:Vector3) -> Vector3:
	return dir
func sprint_m(dir:Vector3) -> Vector3:
	return dir
func dir_m(dir:Vector3) -> Vector3:
	return dir
func jump_anyways() -> bool:
	return false
func vel_m(vel:Vector3) -> Vector3:
	return vel


