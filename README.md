# Baba is You
- **Baba is You** is an award-winning puzzle game where you can change the rules by which you play. 
- In every level, the rules themselves are present as blocks you can interact with. By manipulating them, you can change how the level works and cause surprising, unexpected interactions!
- This is a clone of that famous game.
- **Play** : [Play](https://brickster241.itch.io/baba-is-you)
- **Demo** : [Demo](https://drive.google.com/file/d/1mT23zIoIDCJtPaU6Lc1bHkX1J38x0J4A/view?usp=sharing)

 <p align="center">
  <img src="https://github.com/brickster241/Baba-Is-You/assets/65897987/7f029a3a-8add-4037-b750-a54b72c42ef6"/>
</p>

![Baba-Is-You](https://github.com/brickster241/Baba-Is-You/assets/65897987/dd631b4d-af13-4dcb-bc6e-9f294551b79c)

### Features : 
- Different Nouns in Levels : **BABA, WALL, FLAG, ROCK**.
- Code Architecture Patterns Used : **State Pattern, Singleton Pattern, Top-Down Code Architecture**.
- **Property State Machine** which has a **single dominant property** , but holds multiple properties at the same time. Insted of switching property, you add or remove a property. 
- Preference Order is **YOU > STOP > PUSH > WIN > NONE**.

- Different Types of Properties :
    - **YOU**  : The character you are in control of. Multiple Blocks can have the property YOU.
    - **STOP** : All Blocks with this property cannot be moved.
    - **PUSH** : All Blocks with this property can be pushed. Multiple adjacent Blocks can be pushed.
    - **WIN**  : Blocks with this property are pass-through, but when YOU passes through WIN, the level Finishes.
    - **NONE** : Blocks with no properties cannot be pushed, moved, stopped. They are just pass-through.

- Rules can be modified at run time. General Knowledge : 
    - Rules can only be of type **NOUN Is PROPERTY**. 
    - **IS** is an operator block. Rules can only be around operator blocks.
    - Only way to create rules is **Left to Right / Top to Down**.
    - Highlighted Rules are Active.
- Scene Transitions for smoother gameplay.
