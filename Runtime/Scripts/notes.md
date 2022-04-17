# Voxel System

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


todo
- [ ] publish online
  - [ ] remove dependency on Kutil
  - [ ] paid version?
  - [ ] lisence
  - [ ] sample scenes
  - [ ] tests
  - [ ] api docs
- [ ] Voxel World
  - [ ] Chunk management
    - [ ] auto load around cameras?
  - [ ] LODs
    - [ ] Octtree
  - [ ] show in editor
  - [ ] edit tools
  - [ ] Generators
    - [ ] simple flat
    - [ ] node graph?
    - [ ] scripting api?
- [ ] Voxel Model
  - [ ] like world but no chunks
- [ ] Voxel Data
  - [ ] stores just an int? uint16? float?
  - [ ] Materials
    - [ ] multiple options
      - [ ] seperate material per face (but batch when not seperate)
      - [ ] uv position offset
      - [ ] world uv offset (so a texture can spread across multiple voxels)
      - [ ] tint?
      - [ ] seperate layer
- [ ] mesh generation
  - [ ] options
    - [ ] simple cubes
    - [ ] greedy cubes
    - [ ] quick greedy cubes
    - [ ] marching cubes
  - [ ] non perfect cubes?
- [ ] collision
  - [ ] cube or mesh?
- [ ] save and load
  - [ ] voxel file format
  - [ ] compression vs load speed
  - [ ] import from multiple sources
- [ ] testing
  - [ ] performance test
    - [ ] many voxels
    - [ ] lots of changes per frame 
  - [ ] smoke test all features
- [ ] extra stuff?
  - [ ] multiplayer




## Voxel Plugin notes
voxel plugin unity
port to unity
have all major features
and some others

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






