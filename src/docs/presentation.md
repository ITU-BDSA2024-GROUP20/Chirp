<style>
    hh { color: black; background-color: yellow; font-weight: bolder; padding: 3px }
</style>

# Presentation
>## Quick tour
> - Register through GitHub
> - Cheep a Cheep
> - Check some else's timeline
> - Follow someone
> - Check My Timeline
> - GoTo About Me
> - Forget Me

>## Demo of Blocking and Page buttons
> - Register normal account
> - Proof that deleted users cannot be followed nor blocked
> - Follow two users
> - GoTo About Me
> - Proof that two users has been followed
> - Check followed user timeline out
> - Block them
> - GoTo About Me
> - Public timeline, bottom of page
> - Change page, note colouration.

>## Implementation
>>### Forget Me
>> Users that wish to delete themselves off the platform get anonymized, with the tag \[DELETED]
>>
>> Unique <hh>[DELETED]</hh> names are given, due to primary keys not accepting duplicate values.
>>
>> Account recovery was considered, but was left on the cutting room floor, due to true data protective necessities.
>
>>### Page Button
>> Quick access to other pages for less internet equipped users.
>>
>> Lockout buttons that lead to nowhere.
>
>>### Blocking
>> Users can be malicious and/or hostile.
>>
>> Block out unlikable users, to remove them from most pages.
>>
>> Blocking and Following exclusion.

> ## Testing
>

Simon: Fix page buttons to not be able to proceed past a page with less than 32 cheeps.