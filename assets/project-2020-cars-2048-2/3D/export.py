# GENERATE SPRITES
#
import time
import bpy
from bpy.props import BoolProperty

# https://blender.stackexchange.com/questions/147105/python-scripting-how-to-hide-from-viewport-all-child-objects-linked-through-par
def hide_children(self, context):
    self.hide_viewport = context
    self.hide_render = context
    list_of_children = self.children
    for obj in list_of_children:
        hide_children(obj, context)

# https://blender.stackexchange.com/questions/120517/rendering-a-cube-as-png-file-using-blenders-python-api
def render_to_png(filepath):
    scene = bpy.context.scene
    # render settings
    scene.render.image_settings.file_format = 'PNG'
    scene.render.filepath = filepath
    # render
    bpy.ops.render.render(write_still = 1)

# https://blender.stackexchange.com/questions/120517/rendering-a-cube-as-png-file-using-blenders-python-api
# https://blender.stackexchange.com/questions/132825/python-selecting-object-by-name-in-2-8   
def set_camera(cameraname, x, y, type):
    scene = bpy.context.scene
    # Create the camera
    cam = bpy.context.scene.objects[cameraname];
    cam.type = type
    scene.camera = cam
    # dim
    render = bpy.context.scene.render
    render.resolution_x = x
    render.resolution_y = y
    
def hide_all_tiles():
    list_of_tiles = ["0004","0008","0016", "0032", "0064", "0128", "0256", "0512", "1024", "2048"];
    for name in list_of_tiles:
        hide_children(bpy.context.scene.objects[name], True)
    
def hide_all_cubes():
    list_of_cubes = ["0002","0004","0008","0016", "0032", "0064", "0128", "0256", "0512", "1024", "2048"];
    for name in list_of_cubes:
        hide_children(bpy.context.scene.objects["cube.top."+name], True)
        
path = "D:/";
basecube = bpy.context.scene.objects["cube.top.0002"];
list_of_tiles = ["0004","0008","0016", "0032", "0064", "0128", "0256", "0512", "1024", "2048"];
# prepare
hide_all_tiles()
hide_all_cubes()
for name in list_of_tiles:
    tile = bpy.data.objects[name];
    cube = bpy.context.scene.objects["cube.top."+name];
    hide_all_cubes()
    hide_children(tile, False)
    # preview
    set_camera("Camera.preview", 512, 256, "PERSP")
    render_to_png(path + name + "-preview.png")
    # tile
    hide_children(cube, False)
    set_camera("Camera", 600, 1550, "ORTHO")
    render_to_png(path + name + ".png")
    # feedback
    bpy.ops.wm.redraw_timer(type='DRAW_WIN_SWAP', iterations=1)
    time.sleep(0.5)
# restore
hide_all_cubes()
hide_children(basecube, False)