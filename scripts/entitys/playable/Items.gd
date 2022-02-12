extends Node

onready var player = $".."

var items = {}

var mods = [[], [], [], [], [], [], [], [], [], []]

func refresh_payloads() -> void:
	var payloads = get_children()
	for payload in payloads:
		match payload.pt:
			PayloadTypes.loadType.STAT:
				payload.action(player)
				payload.queue_free()
			PayloadTypes.loadType.NULL:
				pass
			PayloadTypes.loadType.CUSTOM:
				pass
			PayloadTypes.loadType.MOD:
				var mt = payload.mt
				if mt >= 0:
					mods[mt].append(payload.actionSc)
				payload.queue_free()
			_:
				print("payload type NULL")

func print_items() -> void:
	var i:int = 0
	for item in items:
		for j in items[item]:
			print(str(i)+": "+str(item))
			i+=1

func _ready() -> void:
	pass

func execute_mods(index:int, input):
	var temp = input
	for mod in mods[index]:
		temp = mod.action(temp, player)
	return temp

func forwards_m(dir:Vector3) -> Vector3:
	return execute_mods(PayloadTypes.modType.FORWARDS, dir)
func backwards_m(dir:Vector3) -> Vector3:
	return execute_mods(PayloadTypes.modType.BACKWARDS, dir)
func left_m(dir:Vector3) -> Vector3:
	return execute_mods(PayloadTypes.modType.LEFT, dir)
func right_m(dir:Vector3) -> Vector3:
	return execute_mods(PayloadTypes.modType.RIGHT, dir)
func jump_m(dir:Vector3) -> Vector3:
	return execute_mods(PayloadTypes.modType.JUMP, dir)
func sneak_m(dir:Vector3) -> Vector3:
	return execute_mods(PayloadTypes.modType.SNEAK, dir)
func sprint_m(dir:Vector3) -> Vector3:
	return execute_mods(PayloadTypes.modType.SPRINT, dir)
func dir_m(dir:Vector3) -> Vector3:
	return execute_mods(PayloadTypes.modType.DIR, dir)
func jump_anyways() -> bool:
	return execute_mods(PayloadTypes.modType.JUMP_ANYWAYS, false)
func vel_m(vel:Vector3) -> Vector3:
	return execute_mods(PayloadTypes.modType.VEL, vel)


