extends Node

export(PayloadTypes.loadType) var pt = PayloadTypes.loadType.NULL
export(PayloadTypes.modType) var mt = PayloadTypes.modType.NULL
export(Script) var actionSc


func action(player) -> void:
	actionSc.action(player)
