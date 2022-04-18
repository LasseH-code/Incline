tool
extends Tabs

var mov_a

func _on_Stick_relative_mov(mov):
	print("absolute: ", mov_a, "; relative: ", mov)


func _on_Stick_absolute_mov(absolute):
	mov_a = absolute
