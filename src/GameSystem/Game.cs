namespace Assignment.GameSystem
{
    /// <summary>
    /// Manages the game flow and owns all cross-character interactions.
    /// </summary>
    /// <remarks>
    /// Responsibilities: the character roster, turn actions (equip, defend, attack,
    /// heal, abilities), the demo scenario, and all console output (moved out of
    /// <see cref="Character"/> so the domain stays UI-free). Message order and texts
    /// match the pre-refactor program output.
    /// </remarks>
    public sealed class Game
    {
        private readonly List<Character> _characters = new();
        private readonly IGameLogger _logger;

        /// <summary>
        /// Gets the read-only roster of characters.
        /// </summary>
        /// <value>The read-only roster of characters.</value>
        /// <remarks>
        /// Characters can only join through <see cref="CreateCharacter"/> and <see cref="AddCharacter"/>.
        /// </remarks>
        public IReadOnlyList<Character> Characters => _characters.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="logger">The logger used for game output, or <c>null</c> for the console logger.</param>
        public Game(IGameLogger? logger = null)
        {
            _logger = logger ?? new ConsoleGameLogger();
        }

        /// <summary>
        /// Creates a character and adds it to the roster.
        /// </summary>
        /// <param name="name">The name of the character.</param>
        /// <param name="maxHealth">The maximum health of the character.</param>
        /// <param name="baseArmor">The base armor of the character.</param>
        /// <param name="baseAttack">The base attack of the character.</param>
        /// <returns>The created character.</returns>
        public Character CreateCharacter(string name, int maxHealth, int baseArmor, int baseAttack)
        {
            var character = new Character(name, maxHealth, baseArmor, baseAttack);
            _characters.Add(character);
            return character;
        }

        /// <summary>
        /// Adds an existing character to the roster.
        /// </summary>
        /// <remarks>
        /// A character already present in the roster is ignored.
        /// </remarks>
        /// <param name="character">The character to add.</param>
        public void AddCharacter(Character character)
        {
            ArgumentNullException.ThrowIfNull(character);
            if (!_characters.Contains(character))
                _characters.Add(character);
        }

        /// <summary>
        /// Equips a character with an item and reports it.
        /// </summary>
        /// <param name="character">The character to equip.</param>
        /// <param name="equipment">The equipment to equip.</param>
        public void Equip(Character character, Equipment equipment)
        {
            ArgumentNullException.ThrowIfNull(character);
            ArgumentNullException.ThrowIfNull(equipment);
            character.Equip(equipment);
            _logger.LogLine($"{character.Name} equipped {equipment.Name} (+{equipment.AttackBonus} ATK, +{equipment.ArmorBonus} ARM).");
        }

        /// <summary>
        /// Puts a character into a defensive stance and reports it.
        /// </summary>
        /// <param name="character">The character to defend with.</param>
        public void Defend(Character character)
        {
            ArgumentNullException.ThrowIfNull(character);
            character.Defend();
            _logger.LogLine($"{character.Name} braces for impact, increasing armor temporarily.");
        }

        /// <summary>
        /// Performs a standard attack of one character against another.
        /// </summary>
        /// <remarks>
        /// A defeat (if any) is announced before the attack line - the same order
        /// as the original <c>Character.Attack</c> implementation.
        /// </remarks>
        /// <param name="attacker">The attacking character.</param>
        /// <param name="target">The target character.</param>
        /// <returns>The damage dealt to the target.</returns>
        public int Attack(Character attacker, Character target)
        {
            ArgumentNullException.ThrowIfNull(attacker);
            ArgumentNullException.ThrowIfNull(target);
            int damage = attacker.Attack(target);
            AnnounceDefeatIfNeeded(target);
            _logger.LogLine($"{attacker.Name} attacks {target.Name} for {damage} damage!");
            return damage;
        }

        /// <summary>
        /// Heals a character and reports it.
        /// </summary>
        /// <param name="character">The character to heal.</param>
        /// <param name="amount">The amount of health to restore.</param>
        public void Heal(Character character, int amount)
        {
            ArgumentNullException.ThrowIfNull(character);
            character.Heal(amount);
            _logger.LogLine($"{character.Name} heals for {amount} HP. Current HP: {character.Health}/{character.MaxHealth}");
        }

        /// <summary>
        /// Uses an ability object of one character against another.
        /// </summary>
        /// <remarks>
        /// The "uses ability" line comes first and the defeat line (if any) after it -
        /// the same order as the original implementation.
        /// </remarks>
        /// <param name="user">The character using the ability.</param>
        /// <param name="ability">The ability to use.</param>
        /// <param name="target">The character targeted by the ability.</param>
        /// <returns>The damage dealt to the target.</returns>
        public int UseAbility(Character user, IAbility ability, Character target)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(ability);
            ArgumentNullException.ThrowIfNull(target);
            _logger.LogLine($"{user.Name} uses special ability: [{ability.Name}] on {target.Name}!");
            int damage = user.UseAbility(ability, target);
            AnnounceDefeatIfNeeded(target);
            return damage;
        }

        /// <summary>
        /// Uses an ability identified by name of one character against another.
        /// </summary>
        /// <remarks>
        /// String-based convenience overload with the same behaviour as the original demo.
        /// </remarks>
        /// <param name="user">The character using the ability.</param>
        /// <param name="abilityName">The name of the ability to use.</param>
        /// <param name="target">The character targeted by the ability.</param>
        /// <returns>The damage dealt to the target.</returns>
        public int UseAbility(Character user, string abilityName, Character target) =>
            UseAbility(user, new Ability(abilityName), target);

        /// <summary>
        /// Announces the defeat of a character if it is defeated.
        /// </summary>
        /// <param name="character">The character to check.</param>
        private void AnnounceDefeatIfNeeded(Character character)
        {
            if (character.IsDefeated)
                _logger.LogLine($"{character.Name} has been defeated!");
        }

        /// <summary>
        /// Runs the demo combat scenario.
        /// </summary>
        /// <remarks>
        /// The original program combat scenario, now owned by <see cref="Game"/>:
        /// Arthur (warrior) vs Merlin (mage), the equip phase, a blank line,
        /// then defend, attacks, heal and Fireball.
        /// </remarks>
        public void RunDemoScenario()
        {
            var warrior = CreateCharacter("Arthur", maxHealth: 100, baseArmor: 5, baseAttack: 15);
            var mage = CreateCharacter("Merlin", maxHealth: 70, baseArmor: 2, baseAttack: 25);

            var magicStaff = new Equipment("Staff of Fire", attackBonus: 10, armorBonus: 0);
            var steelShield = new Equipment("Steel Shield", attackBonus: 0, armorBonus: 10);

            Equip(mage, magicStaff);
            Equip(warrior, steelShield);
            _logger.LogLine();

            // Combat simulation
            Defend(warrior);
            Attack(mage, warrior); // Armor reduces damage
            Attack(warrior, mage);
            Heal(mage, 15);
            UseAbility(mage, new Ability("Fireball"), warrior);
        }
    }
}
