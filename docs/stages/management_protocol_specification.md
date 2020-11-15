# Management Protocol Specification

Defines the messages sent from Management System to Network Nodes in order modify MPLS-Table.

## Synopsis

General format of command entered in Management System:

`<target> <keyword> <params> <options>`

**Target**

Alias for Ip address of a target Network Node.

**Keyword**

For now, two keyword are defined in the protocol:

- `add` for adding a table row.
- `delete` for deleting a table row.

**Params**

Each keyword has its own parameters.

For `add` and `delete` they define the specific row, on which we want to run the action.

**Options**

Options are not required in a command, but they can add a specific functionality to a keyword.

For example adding a `-100` at the end of command with `add` keyword will add a next layer label to the 'out link' golumn.

## The add command

**General format:**

`<target> add <inLink> <inLabel> <outLink> <outLabel> -<nextLayerLabel>`

As already mentioned you can omit `-<nextLayerLabel>` part.

`.` used as `<outLink>` or `<outLabel>` means that the field value should be `-`. See **Examples** section.

**Example**

In order to add a row like this:

| in link | in label | out link | out label |
| :-----: | :------: | :------: | :-------: |
|    1    |   100    |  2:201   |    101    |

to the MPLS-Table of Network Node with "R1" alias.

Use the following command:

`R1 add 1 100 2 101 -201`

## The delete command

**General format:**

`<target> delete <inLink> <inLabel> <outLink> <outLabel>`

**Example**

In order to delete a row like this:

| in link | in label | out link | out label |
| :-----: | :------: | :------: | :-------: |
|    1    |   100    |  2:201   |    101    |

from the  MPLS-Table of Network Node with "R1" alias.

Use the following command:

`R1 delete 1 100 2 101 -201`

## Examples

If we want to create MPLS-Table as  shown below in Network Node with alias "R1", we use commands.

| in link | in label | out link | out label |
| :-----: | :------: | :------: | :-------: |
|    1    |   100    |    2     |    101    |
|    2    |   103    |    -     |     -     |
|    1    |   104    |  3:201   |    105    |
|    2    |   100    |    3     |    103    |

`R1 add 1 100 2 101`

`R1 2 103 . .`

`R1 1 104 3 105 -201`

`R1 2 100 3 103`

