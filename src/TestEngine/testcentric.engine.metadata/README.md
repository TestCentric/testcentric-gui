# TestCentric Engine Metadata Assembly

This assembly incorporates selected classes from Mono.Cecil along with 
our own `CecilExtensions` class, which adds some functionality.

It implements only the functionality needed by the TestCentric engine
and will not be enhanced to add capabilities of a more general nature.
Of course, others are welcome to use it if it meets their needs.

## Why not just use Mono.Cecil?

Orinally, we used Mono.Cecil. However, this presents a few problems.

1. When testing an application, which uses Mono.Cecil itself, a conflict
   of versions can easily arise.

2. In order to support legacy target runtimes, older versions of Cecil
   would have to be used, while newer versions are needed for newer
   runtimes. This originally led us to having to make use of multiple
   versions of the assembly. Our own metadata assembly is tailored so 
   as to build under all the runtimes we support.

3. The footprint of the full version of Mono.Cecil is greater than we 
   would like to have in our agents.

## Nature of Changes / Omissions

All of Mono.Cecil's ability to modify assemblies has been omitted,
since it is not required by TestCentric. All analysis of the code
itself is also omitted for the same reason.

In general, the intended use of this assembly is to examine a file
in order to determine the proper runtime environment for loading and
running the tests that are contained.
