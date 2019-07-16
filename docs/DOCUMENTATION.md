## Documentation standards
When we work with `dotnet` our documentation will be in construtions the following structure

### Summary
Description about purpose, Display when hover on the method/property.

### Parameters
Each parameter needs a description to clarify it's role.
Display when you need to insert it.

### Returns
Description about the return value

### Exceptions
Description about the types of exceptions which are possible to occur when method/property called and what are the conditions to receive it.

*Clarification*: exceptions is part of the signature of the method/property in spite of it is technically not  part of it.

### Remarks
Important notes which are important for the user of the method/property to understand it somehow.

*Clarification*: We should not use it often, because the signature suppose to be clear enough.

## Use

* For method, use all parts
* For property, Use `Summary, Exceptions and Remarks`

**An example of a full documentation**

*Follow the alignment policy*

```cs
//
// Summary:
//     Here we will write our summary,
//     which will be display when hover a method/property
//
// Parameters:
//   myParameter:
//     Describtion of the parameter
//
// Returns:
//     return myParameter as a string.
//
// Exceptions:
//   System.ArgumentNullException:
//     myParameter is null.
//
// Remarks:
//     remarks about the method/property which 
//     is important to understand how to use it properly
public string MyMethod(int myParameter);
```