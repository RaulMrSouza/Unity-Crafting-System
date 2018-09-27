# Simple Craft System
Crafting System and First Person Object placement for Unity

Playable WebGL: http://RaulMrSouza.github.io/SimpleCraftWebGL

Package: https://github.com/RaulMrSouza/SimpleCraft/blob/master/SimpleCraft.unitypackage

Asset Store(old version): https://assetstore.unity.com/packages/templates/systems/simple-craft-system-107967

Video: https://www.youtube.com/watch?v=V9Pm6ZkJ0Yc

The gameplay idea is that the player will use Tools to gather Resources in order 
to build Craftable Items or more Tools.

The project purpose is to be simplistic and easy to be expanded. In other words, easy 
to add more resources, tools and craftable items.

It is not required to know scripting to use and expand this asset.

The Demo scene is a good way to see how it works

Now some information about the project:

To properly run the Simple Craft system on a scene it is required to have the 
following prefabs:

- Resources; 
- Craftable Items;
- Tools;
- a Settlement Manager; 
- a Character;

# About Items

Items are the base prefabs in the crafting system, they can be grabbed into 
the inventory and used for other purposes.
 
To create a new item just create an Item scriptableObject using Create->SimpleCraft->Item
and set a Prefab with a Collider and an Item Refence script to be the physical object of 
the Item.

# About Resources

Each Resource has a amount of a certain Item. when gathered with a tool the Items
will be transferred to the player's inventory.

# About Craftable Items

It must have a cost in Items to be crafted. For instance, a table costs planks to be 
crafted, and the plank cost woods.

Each possible craftable item in the scene must be on the Manager's Items structure, 
so that it can be instantiated.

The process of creating a new Craftable Item is equal to create a new item plus 
filling the craft cost, one thing that can be tricky it's the position, the object's
pivot should be right bellow the object components while the detection collider
should be placed where it can validates where the object can be crafted, look at
the already existing craftable Items to have an example.

# About Tools

A tool is a craftable item.

Each tool has a GatherFactor which is used to define which resources the tool 
is good or bad to use at. For example, in the default tools, the axe is better 
to gather wood, the pickaxe better at gold and stone and the hammer is neutral.

The tool requires that the main object or one of its children has a trigger 
collider, which will be used to detect collisions between the tool and resources.

# About the Settlement Manager

It is responsible for the instantiation of craftable Items, therefore the 
instantiation of tools.

Each craftable item, including tools, must be on the Manager's Items structure.

They must be separated in categories, the default are furniture, tools, buildings
and materials, more can be added in the editor.

The Manager also can be used to check the goal of the scene with the prperty 
objectiveItem. Check the Demo scene to better understand this feature.

# About the Character

The default character has a Player.cs script in the camera, already set to work with a tool handler and a fps style character controler
and the canvas objects.

The Player has a inventory of resources that is used to build new Items and can hold a
 specified number of tools. 
