
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SqlOnAir.DotNet.Lib;

namespace SqlOnAir.DotNet.Lib.DataClasses.BaseClasses
{
    public class TicTacToeUserBase : SoAEntityBase
    {
        [Key]
        public string TicTacToeUserId { get; set; } = $"{Guid.NewGuid()}";

        public string? Name { get; set; }
        public string? TicTacToeEmailAddress { get; set; }
        public string? TicTacToeRoles { get; set; }
        public int? IsAI
        {
            get => NOT({AILevel = BLANK()); set { }
        }

        public string? AIStrategies
        {
            get => AILevel != null ? AILevel.AIStrategies : null; set { }
        }

        public string? Notes
        {
            get => AILevel != null ? AILevel.Description : null; set { }
        }

        public string? Attachments { get; set; }
        public string? Status { get; set; }
        public int? LevelNumber { get; set; }
        public string? AILevelDescription
        {
            get => AILevel != null ? AILevel.Description : null; set { }
        }


        private AILevel _aILevel;
        public virtual AILevel AILevel
        {
            get
            {
                if (_aILevel == null && !string.IsNullOrEmpty(AILevelId))
                {
                    if (Context == null)
                    {
                        throw new InvalidOperationException("The context is not set for this entity.");
                    }
                    _aILevel = Context.AILevels.Find(AILevelId);
                    Context.Attach(_aILevel);
                }
                return _aILevel;
            }
            set
            {
                if (_aILevel != value)
                {
                    if (_aILevel != null)
                    {
                        if (_aILevel.Users != null)
                        {
                            _aILevel.Users.Remove(this as TicTacToeUser);
                        }
                    }

                    _aILevel = value;
                    AILevelId = _aILevel?.AILevelId ?? string.Empty;

                    if (_aILevel != null)
                    {
                        if (_aILevel.Users != null)
                        {
                            _aILevel.Users.Add(this as TicTacToeUser);
                        }
                    }
                }
            }
        }

        public string AILevelId { get; set; }

        private ObservableCollection<Game> _gamesAsPlayer;

        public ObservableCollection<Game> GamesAsPlayer
        {
            get
            {
                if (_gamesAsPlayer == null)
                {
                    if (Context == null)
                    {
                        _gamesAsPlayer = new ObservableCollection<Game>();
                    }
                    else
                    {
                        var items = Context.Games.Where(x => x.TicTacToeUserId == this.TicTacToeUserId).ToList<Game>();
                        _gamesAsPlayer = new ObservableCollection<Game>(items);
                        Context.AttachRange(items);
                    }
                    _gamesAsPlayer.CollectionChanged += GamesAsPlayer_CollectionChanged;
                }
                return _gamesAsPlayer;
            }
            private set
            {
                if (_gamesAsPlayer != null)
                {
                    _gamesAsPlayer.CollectionChanged -= GamesAsPlayer_CollectionChanged;
                }
                _gamesAsPlayer = value;
                if (_gamesAsPlayer != null)
                {
                    _gamesAsPlayer.CollectionChanged += GamesAsPlayer_CollectionChanged;
                }
            }
        }

        private void GamesAsPlayer_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems != null)
            {
                foreach (var item in e.NewItems.Cast<Game>())
                {
                    if (item.Player != this)
                    {
                        item.Player = this as Player;
                    }
                }
            }

            if (e?.OldItems != null)
            {
                foreach (var item in e.OldItems.Cast<Game>())
                {
                    if (item.Player == this)
                    {
                        item.Player = null;
                    }
                }
            }
        }

        private ObservableCollection<Game> _gamesAsOpponent;

        public ObservableCollection<Game> GamesAsOpponent
        {
            get
            {
                if (_gamesAsOpponent == null)
                {
                    if (Context == null)
                    {
                        _gamesAsOpponent = new ObservableCollection<Game>();
                    }
                    else
                    {
                        var items = Context.Games.Where(x => x.TicTacToeUserId == this.TicTacToeUserId).ToList<Game>();
                        _gamesAsOpponent = new ObservableCollection<Game>(items);
                        Context.AttachRange(items);
                    }
                    _gamesAsOpponent.CollectionChanged += GamesAsOpponent_CollectionChanged;
                }
                return _gamesAsOpponent;
            }
            private set
            {
                if (_gamesAsOpponent != null)
                {
                    _gamesAsOpponent.CollectionChanged -= GamesAsOpponent_CollectionChanged;
                }
                _gamesAsOpponent = value;
                if (_gamesAsOpponent != null)
                {
                    _gamesAsOpponent.CollectionChanged += GamesAsOpponent_CollectionChanged;
                }
            }
        }

        private void GamesAsOpponent_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems != null)
            {
                foreach (var item in e.NewItems.Cast<Game>())
                {
                    if (item.Opponent != this)
                    {
                        item.Opponent = this as Opponent;
                    }
                }
            }

            if (e?.OldItems != null)
            {
                foreach (var item in e.OldItems.Cast<Game>())
                {
                    if (item.Opponent == this)
                    {
                        item.Opponent = null;
                    }
                }
            }
        }


        protected override void LazyLoadProperties()
        {
            var aILevel = this.AILevel;
            var gamesAsPlayer = this.GamesAsPlayer;
            var gamesAsOpponent = this.GamesAsOpponent;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}