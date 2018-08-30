simple Craft System

The gameplay idea is that the player will use Tools to gather Resources in order 
to build Craftable Items or more Tools.

The project purpose is to be simplistic and easy to be expanded. In other words, easy 
to add more resources, tools and craftable items.

It is not require to know scripting to use and expand this asset.

The Demo scene is a good way to see how it works.

This is a non-professional one person project, you can contact me on my e-mail:
raul.mr.souza@gmail.com

English is not my first language, so sorry for any typo.

Now some information about the project:

To properly run the Simple Craft system on a scene it is required to have the 
following prefabs:

- Resources; 
- Craftable Items;
- Tools;
- a Settlement Manager; 
- a Character;

About Items

Items are the base prefabs in the craft system, they can be grabbed into 
the inventory and used for other purposes.

To create a new item just add a Item Script to a GameObject with a collider,
each item must have a unique name and be placed on the SimpleCraft/Assets/Resources/items/
folder in order to be instantializable by the manager.

About Resources

Each Resource has a amount of a certain Item. when gathered with a tool the Items
will be transferred to the player's inventory.

About Craftable Items

A craftable item is a object with the craftableItem.cs script, it must have a 
cost in items to be crafted. For instance, a table costs planks to be 
crafted, and the plank cost woods.

Each possible craftable item in the scene must be on the Manager's Items structure, 
so that it can be instantiated.

The process of creating a new Craftable Item is equal to create a new item plus 
filling the craft cost, one thing that can be tricky it's the position, the object's
pivot should be right bellow the object components while the detection collider
should be placed where it can validates where the object can be crafted, look at
the already existing craftable Items to have an example.

About Tools

A tool is a craftable item.

Each tool has a GatherFactor which is used to define which resources the tool 
is good or bad to use at. For example, in the default tools, the axe is better 
to gather wood, the pickaxe better at gold and stone and the hammer is neutral.

To create a new tool just add the Tool.cs script component, the tool requires that 
the main object or one of its children has a trigger collider, which will be used to 
detect collisions between the tool and resources.

About the Settlement Manager

It is responsible for the instantiation of craftable Items, therefore the 
instantiation of tools.

Each craftable item, including tools, must be on the Manager's Items structure.

They must be separated in categories, the default are furniture, tools, buildings
and materials, more can be added in the editor.

The Manager also can be used to check the goal of the scene with the prperty 
objectiveBUilding. Check the Demo scene to better understand this feature.

About the Character

The default character has a Player.cs script in the camera, already set to work with a tool handler and a fps style character controler
and the canvas objects.

The Player has a inventory of resources that is used to build new Items and can hold a
 specified number of tools. 


About me

I'm currently working as a computer programmer, but nothing related to game development,
and studying Unity in my free time, I hope to be a game developer one day, but for now,
nothing professional.
