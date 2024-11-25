# cheat-resistant-value
Simple C# wrapper class for values that provides a minimal level of protection against address scanning tools like cheat engine.

It has some limitations, mainly if the user can manage to overwrite all the duplicates and the security backup value, they can indeed still cheat the data, but for a user who is not saavy they will not know how to do this.

**Usage**

Using this class is dead simple. If you have a value you wish to protect:

```cpp
bool isDlcUnlocked;
float playerHighscore;
```
Change it to:
```cpp
CheatResistantValue<bool> isDlcUnlocked;
CheatResistantValue<float> playerHighScore;
```
And they're protected! To read and write the value simply use the `.Get()` and `.Set()` functions.

Its generic and entirely contained within CheatResistantValue.cs, if you would like to know how this works there is an explanation there above the class
