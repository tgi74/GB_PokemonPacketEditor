namespace PokemonPacketCorruptor
{
    internal class AIBattle : Battle
    {
        internal AiAction Ai = new AiAction();

        internal AIBattle()
        {
            Trainer = TrainerBotProfiles.LoadRandomTrainerProfile();
        }

        protected override byte ProcessData(byte data)
        {
            return Ai.Process(this, data);
        }
    }
}