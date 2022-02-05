extends Spatial

export(bool) var confirmation = false # NOT IMPLEMENTED
export(bool) var spinning = false # NOT IMPLEMENTED
export(String) var pickup_id = "" # NOT IMPLEMENTED

signal pickup_interacted(id)

func _ready():
	get_node()
