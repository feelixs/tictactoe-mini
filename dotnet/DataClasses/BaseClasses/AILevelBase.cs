
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SqlOnAir.DotNet.Lib;

namespace SqlOnAir.DotNet.Lib.DataClasses.BaseClasses
{
    public class AILevelBase : SoAEntityBase
    {
        [Key]
        public string AILevelId { get; set; } = $"{Guid.NewGuid()}";

        public string? Name { get; set; }
        public string? PlayerType { get; set; }
        public string? AIStrategies { get; set; }
        public string? AIStrategyNames
        {
            get => null /* Unhandled formula: {AIStrategies} */; set { }
        }

        public int? MinAILevelIndex { get; set; }
        public int? AILevelIndex { get; set; }
        public string? Description { get; set; }
        public int? SortOrder { get; set; }

        private ObservableCollection<TicTacToeUser> _users;

        public ObservableCollection<TicTacToeUser> Users
        {
            get
            {
                if (_users == null)
                {
                    if (Context == null)
                    {
                        _users = new ObservableCollection<TicTacToeUser>();
                    }
                    else
                    {
                        var items = Context.TicTacToeUsers.Where(x => x.AILevelId == this.AILevelId).ToList<TicTacToeUser>();
                        _users = new ObservableCollection<TicTacToeUser>(items);
                        Context.AttachRange(items);
                    }
                    _users.CollectionChanged += Users_CollectionChanged;
                }
                return _users;
            }
            private set
            {
                if (_users != null)
                {
                    _users.CollectionChanged -= Users_CollectionChanged;
                }
                _users = value;
                if (_users != null)
                {
                    _users.CollectionChanged += Users_CollectionChanged;
                }
            }
        }

        private void Users_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems != null)
            {
                foreach (var item in e.NewItems.Cast<TicTacToeUser>())
                {
                    if (item.AILevel != this)
                    {
                        item.AILevel = this as AILevel;
                    }
                }
            }

            if (e?.OldItems != null)
            {
                foreach (var item in e.OldItems.Cast<TicTacToeUser>())
                {
                    if (item.AILevel == this)
                    {
                        item.AILevel = null;
                    }
                }
            }
        }


        protected override void LazyLoadProperties()
        {
            var users = this.Users;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}