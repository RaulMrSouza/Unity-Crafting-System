# SimpleCraft

The gameplay idea is that the player will use Tools to gather Resources in order 
to build Craftable Itens or more Tools.

The project purpose is to be simplistic and easy to be expanded. In other words, easy 
to add more resources, tools and craftable itens.

The Demo scene is a good way to see how it works.

Video: https://www.youtube.com/watch?v=m25qGzVMHlQ

This is a non-professional one person project, you can contact me on my e-mail:
raul.mr.souza@gmail.com

English is not my first language, so sorry for any typo.

Now some information about the project:

To properly run the Simple Craft system on a scene it is required to have the 
following prefabs:

- Resources; 
- Craftable Itens;
- Tools;
- a Manager; 
- a Character;

About Resources

Each Resource has a type (the default types are wood, stone, gold, iron and plank), 
the set of types can be easily expanded. For example, to add marble just change:



public enum Type{
			wood,
			stone,
			iron,
			gold,
			plank
		}
    
    
To:


public enum Type{
			wood,
			stone,
			iron,
			gold,
			plank,
			marble 
		}

In the Resource.cs script and recompile the project.

About Craftable Itens

A craftable item is a object with the craftableItem.cs script, it must have a cost in 
resources in order to be built.

Each possible craftable item in the scene must be on the Manager's Itens array, 
so that it can be instanciated.

The craftable item's name must be unique, it is used as a key in the system.

About Tools

A tool is a craftable item.

Each tool has a GatherFactor which is used to define which resources the tool is good or 
bad to use at. For example, in the default tools, the axe is better to gather wood, 
the pickaxe better at gold and stone and the hammer is neutral.

About the Manager

It is responsible for the instatiation of craftable itens, therefore the instatiation of tools.

Each craftable item, including tools, must be on the Manager's Itens array.

The Manager also can be used to check the goal of the scene with the prperty 
objectiveBUilding. Check the Demo scene to better understand this feature.

About the Character

The default character has a Player.cs script in the camera, already set to work with a tool handler and a fps style character controler
and the canvas objects.

The Player has a inventory of resources that is used to build new itens and can hold a
 specified number of tools. 
