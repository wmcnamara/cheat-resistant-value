using System;


//This is the behaviour the CRV will use if the value has been cheated and cannot be recovered
public enum CRVCheatResponseBehaviour
{
    RevertToDefault, //If all the backup values are modified and the correct value cannot be recovered the CRV will revert to the default construction of T (new T())
    RevertToPreviousValue, //This is far less secure, and should be avoided. If used the CRV will revert to the previous value used. Use only when reverting to default will cause damage
}

/* 
 * The cheat resistant data type is intended to be used as an alternative to direct in memory values that represent a value that may be cheated.
 * It works by using three copies of the same value, and a hash of the correct value, that are compared and validated against each other to throw off memory editing programs like cheat engine.
 * To use it simply replace the value used to store whatever value you wish to have some resistance against, and use CheatResistantValue instead for the same type.
 * 
 * This makes it much harder for a cheater as they wont be able to see all of the values they can edit easily at the same time to cheat it with a single cheat engine instance unless they are very skilled at scanning.
 * You will have a small memory and performance hit (uses (4 * sizeof(T)) + (2 * sizeof(int)) more memory than the direct value and setting it is slightly slower due to validation), but for a couple values it doesnt matter
 * 
 * This has three security features:
 * 3 copies of the same value are stored, which if only one is modified, it will recover the correct value
 * The actual validation is done with hashes, instead of a naive double value comparison that is easily hacked away by a saavy CE user who knows to whittle down addresses and then change all that appear
 * Has a revert behaviour in the event that the value is cheated and cannot be recovered that is customizable
*/
public struct CheatResistantValue<T> where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
{
    CheatResistantValue(T initialValue, CRVCheatResponseBehaviour behaviour = CRVCheatResponseBehaviour.RevertToDefault)
    {
        first = initialValue;
        second = initialValue;
        third = initialValue;

        backupValue = initialValue;

        cheatResponseBehaviour = behaviour;

        storedValueHash = first.GetHashCode();
    }

    public T Get()
    {
        Validate();

        return first;
    }

    public void Set(T newValue)
    {
        Validate();

        backupValue = first;

        first = newValue;
        storedValueHash = first.GetHashCode();
    }

    private void Validate()
    {
        //Perform the actual comparison with a hash. 
        //If the hash isnt used for a comparison, a user of CE who is value scanning for whatever they wish to change could simply change all of the values that are shown in the CE window.
        //Not doing this would make a three way comparison succeed in such a case, breaking the system.
        int realValueHash = first.GetHashCode();

        if (realValueHash == storedValueHash)
        {
            return;
        }

        //Logic to handle a situation where realValue is cheated starts here

        //Determine which value is the wrong one by matching against other two, and try to recover the correct value

        //Second is still valid; revert to it
        if (second.GetHashCode() == storedValueHash)
        {
            first = second;
            third = second;

            return;
        }

        //Third is still valid; revert to it
        if (third.GetHashCode() == storedValueHash)
        {
            first = third;
            second = third;

            return;
        }

        //If we are here, it means all of the values have been modified, and the original value cannot be recovered. Go to the fallback cheat response behaviour
        switch (cheatResponseBehaviour)
        {
            case CRVCheatResponseBehaviour.RevertToDefault:
                first = new T();
                second = first;
                third = first;

                storedValueHash = first.GetHashCode();
                break;

            case CRVCheatResponseBehaviour.RevertToPreviousValue:
                first = backupValue;
                second = backupValue;
                third = backupValue;

                storedValueHash = first.GetHashCode();
                break;
        }
    }

    private T first;
    private T second;
    private T third;

    private T backupValue;

    private CRVCheatResponseBehaviour cheatResponseBehaviour;

    private int storedValueHash;
}
