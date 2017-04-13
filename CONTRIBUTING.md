How to Contribute to aima-csharp
==========================

Thanks for considering contributing to `aima-csharp`! Whether you are an aspiring [Google Summer of Code](https://summerofcode.withgoogle.com/organizations/5663121491361792/) student, or an independent contributor, here is a guide to how you can help:

## Read the Code and Start on an Issue

- First, read and understand the code to get a feel for the extent and the style.
- Look at the [issues](https://github.com/aimacode/aima-csharp/issues) and pick one to work on.
- One of the issues is that some algorithms are missing from the [list of algorithms](/README.md#index-of-implemented-algorithms).

## New and Improved Algorithms

- Implement functions that were in the third edition of the book but were not yet implemented in the code. Check the [list of pseudocode algorithms (pdf)](https://github.com/aimacode/pseudocode/blob/master/aima3e-algorithms.pdf) to see what's missing.
- As we finish chapters for the new fourth edition, we will share the new pseudocode in the [`aima-pseudocode`](https://github.com/aimacode/aima-pseudocode) repository, and describe what changes are necessary.
We hope to have a `algorithm-name.md` file for each algorithm, eventually; it would be great if contributors could add some for the existing algorithms.


Contributing a Patch
====================

1. Submit an issue describing your proposed change to the repo in question (or work on an existing issue).
1. The repo owner will respond to your issue promptly.
1. Fork the desired repo, develop and test your code changes.
1. Submit a pull request.

Reporting Issues
================

- Under which versions of Visual Studio does this happen?

- Is anybody working on this?

# Choice of Programming Languages

Are we right to concentrate on Java and Python versions of the code? I think so; both languages are popular; Java is
fast enough for our purposes, and has reasonable type declarations (but can be verbose); Python is popular and has a very direct mapping to the pseudocode in the book (but lacks type declarations and can be slow). The [TIOBE Index](http://www.tiobe.com/tiobe_index) says the top seven most popular languages, in order, are:

        Java, C, C++, C#, Python, PHP, Javascript

So it might be reasonable to also support C++/C# at some point in the future. It might also be reasonable to support a language that combines the terse readability of Python with the type safety and speed of Java; perhaps Go or Julia. I see no reason to support PHP. Javascript is the language of the browser; it would be nice to have code that runs in the browser without need for any downloads; this would be in Javascript or a variant such as Typescript.

There is also a `aima-lisp` project; in 1995 when we wrote the first edition of the book, Lisp was the right choice, but today it is less popular (currently #31 on the TIOBE index).
