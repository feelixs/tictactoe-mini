
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SqlOnAir.DotNet.Lib;

namespace SqlOnAir.DotNet.Lib.DataClasses.BaseClasses
{
    public class GameBase : SoAEntityBase
    {
        [Key]
        public string GameId { get; set; } = $"{Guid.NewGuid()}";

        public string? Name
        {
            get => null /* Unhandled formula: CONCATENATE({Createdtime},":",{Player},":vs:",{Opponent}) */; set { }
        }

        public string? PlayerName
        {
            get => Player != null ? Player.Name : null; set { }
        }

        public int? IsAI
        {
            get => IF({OpponentTypeName = BLANK(),FALSE(),TRUE()); set { }
        }

        public string? OpponentType
        {
            get => Opponent != null ? Opponent.AILevel : null; set { }
        }

        public string? OpponentTypeName
        {
            get => null /* Unhandled formula: {OpponentType} */; set { }
        }

        public string? AIStrategies
        {
            get => Opponent != null ? Opponent.AIStrategies : null; set { }
        }

        public DateTime? Createdtime { get; private set; }
        public string? Notes { get; set; }
        public string? Attachments { get; set; }
        public string? Status { get; set; }
        public string? AILevel
        {
            get => Opponent != null ? Opponent.AILevel : null; set { }
        }


        private TicTacToeUser _player;
        public virtual TicTacToeUser Player
        {
            get
            {
                if (_player == null && !string.IsNullOrEmpty(PlayerId))
                {
                    if (Context == null)
                    {
                        throw new InvalidOperationException("The context is not set for this entity.");
                    }
                    _player = Context.TicTacToeUsers.Find(PlayerId);
                    Context.Attach(_player);
                }
                return _player;
            }
            set
            {
                if (_player != value)
                {
                    if (_player != null)
                    {
                        if (_player.GamesAsPlayer != null)
                        {
                            _player.GamesAsPlayer.Remove(this as Game);
                        }
                    }

                    _player = value;
                    PlayerId = _player?.TicTacToeUserId ?? string.Empty;

                    if (_player != null)
                    {
                        if (_player.GamesAsPlayer != null)
                        {
                            _player.GamesAsPlayer.Add(this as Game);
                        }
                    }
                }
            }
        }

        public string PlayerId { get; set; }

        private TicTacToeUser _opponent;
        public virtual TicTacToeUser Opponent
        {
            get
            {
                if (_opponent == null && !string.IsNullOrEmpty(OpponentId))
                {
                    if (Context == null)
                    {
                        throw new InvalidOperationException("The context is not set for this entity.");
                    }
                    _opponent = Context.TicTacToeUsers.Find(OpponentId);
                    Context.Attach(_opponent);
                }
                return _opponent;
            }
            set
            {
                if (_opponent != value)
                {
                    if (_opponent != null)
                    {
                        if (_opponent.GamesAsOpponent != null)
                        {
                            _opponent.GamesAsOpponent.Remove(this as Game);
                        }
                    }

                    _opponent = value;
                    OpponentId = _opponent?.TicTacToeUserId ?? string.Empty;

                    if (_opponent != null)
                    {
                        if (_opponent.GamesAsOpponent != null)
                        {
                            _opponent.GamesAsOpponent.Add(this as Game);
                        }
                    }
                }
            }
        }

        public string OpponentId { get; set; }


        protected override void LazyLoadProperties()
        {
            var player = this.Player;
            var opponent = this.Opponent;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}