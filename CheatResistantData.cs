using System;

/* 
 * The cheat resistant data type is intended to be used as an alternative to direct in memory values that represent a value that may be cheated.
 * It works by using three copies of the same value, that are compared and validated against each other to throw off memory editing programs like cheat engine.
 * To use it simply replace the value used to store whatever value you wish to have some resistance against, and use CheatResistantValue instead for the same type.
 * Users can bypass this by overwriting the three clone numbers, and the previousBackupValue. The previousBackupValue makes it much harder to crack for a newbie but its still possible.
 * 
 * You will have a small memory and performance hit (uses 3 * sizeof(T) more memory than the direct value and setting it is slightly slower due to validation), but for a couple values it doesnt matter
*/
struct CheatResistantValue<T> where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
{
    public T Get()
    {
        Validate();

        return first;
    }

    public void Set(T value)
    {
        Validate();

        previousBackupValue = first;

        first = value;
        second = value;
        third = value;
    }

    private void Validate()
    {
        if (!first.Equals(second) && second.Equals(third))
        {
            return;
        }

        //Determine which value is the wrong one by matching against other two

        //First is incorrect
        if (!first.Equals(second) && second.Equals(third))
        {
            first = second;
            return;
        }

        //Second is incorrect
        if (!second.Equals(third) && third.Equals(first))
        {
            second = third;
            return;
        }

        //Third is incorrect
        if (!third.Equals(first) && first.Equals(second))
        {
            third = first;
            return;
        }

        //If we are here, two or more are incorrect. That means that atleast two were overwritten.
        //In this case, we dont have a reliable gauge of the correct value, and we can simply revert to the previous as a security measure

        first = previousBackupValue;
        second = previousBackupValue;
        third = previousBackupValue;
    }

    private T first;
    private T second;
    private T third;

    private T previousBackupValue;
}