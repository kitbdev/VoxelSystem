voxel plugin unity
port to unity
have all major features
and some others


todo
[ ] everything



Voxel World
- Preview
	- toggle
	- clear world/values/mats
	- set dirty values/mats
	- scale data
	- auto refresh
- Save
	- set save object
	- load from save obj button
	- save/load to file
- General
	- voxel size
	- generator select
	- placable item manager select? (data for generator to use, I think)
	- create automatically option
	- undo redo support
	- custom rebasing 
	- debug 
- World size
	- world width in voxels, render octtree depth(10),custom world bounds 
- Rendering
	- render type: marching cubes, cubic, or suface nets
	- enable(or just for collisions), other settings 
- Materials
	- mode - rgb(color), single index(materials), multiindex(smoothed materials?)
	- config
	- materials
		- voxel material collection
	- UVs
		- global or per voxel or chunk or pack world up
		- scale
	- LOD, Normals, Hardness
- Spawners (spawn prefabs on terrain)
	- set spawner
- Physics
	- simulate, gravity, other rb settings?
- Collisions
	- enable, complex or simple or both, layer
- Navmesh
	- enable, compute, max lod
- LOD settings
	- max lod
	- min lod? certain applications
	- constant option, min update delay, invoker distance threshold
- Performance settings
	- create global pool and threads
	- octtree depth
	- mesh update budget per tick (1000 ms)
	- priorities
		- chunk meshing - collision, visible, vis and col
		- collision cooking, folliage build, instanced mesh culling tree, async edit, mesh merge, render octtree(determines lod)
- Multiplayer
	- enable, interface, sync rate
- Bake (to static meshes)


wants
easy to setup and use
extendable
dynamic LOD, GPU accelerated, multithreaded
editor tools
runtime modification

small voxel renderer (for single chunk objects)
voxel world (for up to infinite terrain)
realtime voxelization? maybe

runtime modifiable and destructable
custom data on any voxel?
rigidbody physics?





