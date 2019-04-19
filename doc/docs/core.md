# EPiServer.Vsf.Core

This project provides a set of base classes and interfaces used in project EPiServer.Vsf.ApiBridge and EPiServer.Vsf.DataExport. The intent is that One can override whole logic with custom implementation.

## Folders structure
    .
    +-- ApiBridge - interfaces and models related to VueStoreFront Api bridge.
    |   +-- Adapter - adapter interfaces for managing users, cars and stock
    |   +-- Endpoint - api bridge interfaces
    |   +-- Model - models related to VueStoreFront Api bridge
    |   |   +-- Authorization - related to authorization 
    |   |   +-- Cart - related to carts 
    |   |   +-- Stock - related to stock
    |   |   +-- User - related to users
    +-- Exporting - set of interfaces related to exporting 
    +-- Mapping - set of interfaces related to exporting 
