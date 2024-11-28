# cheat-resistant-value
Simple C# wrapper class for values that provides a minimal level of protection against address scanning tools like cheat engine.

It has some limitations, mainly if the user can manage to overwrite all the duplicates and the security backup value, they can indeed still cheat the data, but for a user who is not saavy they will not know how to do this.

**Features** 
1. Extremely difficult to bypass cheat detection
2. Auto-attempts to recover the value (And provides custom fallback options if the value cannot be recovered)
3. Easy to use

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

You can use these values with a custom constructor to assign a default to the values, or to change their value fallback behaviour.

*Fallback Behaviours*

CRV has a few different fallback behaviours, which are what the CRV should do in the event that the value is cheated and cannot be recovered.

1. `RevertToDefault`
    This sets the value in the CRV to the default constructor of the CRV type (sets it to default, int = 0, bool = false, etc)

2. `RevertToPreviousValue`
    This attempts to revert the value to the previously stored value within the CRV. This is far more insecure because if a cheater changes the stored previous value, they can trigger an overwrite with this and cheat the value.

    Only use this if reverting to default will result in a catastrophic result

**Knowing when cheating has occured**

Each CRV fires an event that can be subscribed to when cheating is detected.

```cpp
CheatResistantValue<int> crv = new CheatResistantValue<int>(1);

crv.OnCheatDetected += OnCheatDetected;

void OnCheatDetected()
{
    Console.WriteLine("Value cheated!");
}
```

If you want to know when ANY crv is cheated, there is also a static event that is fired for this.

```cpp
CheatResistantValue.OnAnyValueCheatDetected += OnCheatDetected;
 ```

Its generic and entirely contained within CheatResistantValue.cs, if you would like to know how this works there is an explanation there above the class
