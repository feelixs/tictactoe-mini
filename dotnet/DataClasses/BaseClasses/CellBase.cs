
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SqlOnAir.DotNet.Lib;

namespace SqlOnAir.DotNet.Lib.DataClasses.BaseClasses
{
    public class CellBase : SoAEntityBase
    {
        [Key]
        public string CellId { get; set; } = $"{Guid.NewGuid()}";

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AlexesNumber { get; set; }
        public int? IsInRow1
        {
            get => IF(LEFT(RIGHT({AlexesNumber},8),1) = '1',TRUE(),FALSE()); set { }
        }

        public int? IsInRow2
        {
            get => IF(LEFT(RIGHT({AlexesNumber},7),1) = '1',TRUE(),FALSE()); set { }
        }

        public int? IsInRow3
        {
            get => IF(LEFT(RIGHT({AlexesNumber},6),1) = '1',TRUE(),FALSE()); set { }
        }

        public int? IsInColumn1
        {
            get => IF(LEFT(RIGHT({AlexesNumber},5),1) = '1',TRUE(),FALSE()); set { }
        }

        public int? IsInColumn2
        {
            get => IF(LEFT(RIGHT({AlexesNumber},4),1) = '1',TRUE(),FALSE()); set { }
        }

        public int? IsInColumn3
        {
            get => IF(LEFT(RIGHT({AlexesNumber},3),1) = '1',TRUE(),FALSE()); set { }
        }

        public int? IsInLeftRightDiagonal
        {
            get => IF(LEFT(RIGHT({AlexesNumber},2),1) = '1',TRUE(),FALSE()); set { }
        }

        public int? IsInRightLeftDiagonal
        {
            get => IF(LEFT(RIGHT({AlexesNumber},1),1) = '1',TRUE(),FALSE()); set { }
        }

        public bool? IsDailyDouble { get; set; }
        public string? CellPatternCells { get; set; }
        public string? FlipDescription
        {
            get => IF({FlipFromName = {Name},"No Movement", CONCATENATE("From ", {FlipFromName}," to ", {Name})); set { }
        }

        public int? CellIndex { get; set; }
        public int? CellKey { get; set; }
        public string? SampleValue { get; set; }
        public int? X { get; set; }
        public int? Y { get; set; }
        public string? TargetCellForCellPatterns { get; set; }
        public int? SortOrder
        {
            get => null /* Unhandled formula: {CellIndex} */; set { }
        }

        public string? ClockwiseRotateFrom
        {
            get => Clockwise != null ? Clockwise.Name : null; set { }
        }

        public int? ClockwiseRotateFromIndex
        {
            get => Clockwise != null ? Clockwise.CellIndex : null; set { }
        }

        public string? CounterClockwiseRotateFrom
        {
            get => CounterClockwise != null ? CounterClockwise.Name : null; set { }
        }

        public int? CounterClockwiseRotateFromIndex
        {
            get => CounterClockwise != null ? CounterClockwise.CellIndex : null; set { }
        }

        public int? FlipIndex
        {
            get => Flip != null ? Flip.CellIndex : null; set { }
        }

        public string? FlipFromName
        {
            get => Flip != null ? Flip.Name : null; set { }
        }

        public string? CurrentState
        {
            get => CellStates != null ? CellStates.Name : null; set { }
        }

        public string? RotateTranslation { get; set; }

        private Cell _clockwise;
        public virtual Cell Clockwise
        {
            get
            {
                if (_clockwise == null && !string.IsNullOrEmpty(ClockwiseId))
                {
                    if (Context == null)
                    {
                        throw new InvalidOperationException("The context is not set for this entity.");
                    }
                    _clockwise = Context.Cells.Find(ClockwiseId);
                    Context.Attach(_clockwise);
                }
                return _clockwise;
            }
            set
            {
                if (_clockwise != value)
                {
                    if (_clockwise != null)
                    {
                        if (_clockwise. != null)
                        {
                            _clockwise..Remove(this as Cell);
                        }
                    }

                    _clockwise = value;
                    ClockwiseId = _clockwise?.CellId ?? string.Empty;

                    if (_clockwise != null)
                    {
                        if (_clockwise. != null)
                        {
                            _clockwise..Add(this as Cell);
                        }
                    }
                }
            }
        }

        public string ClockwiseId { get; set; }

        private Cell _counterClockwise;
        public virtual Cell CounterClockwise
        {
            get
            {
                if (_counterClockwise == null && !string.IsNullOrEmpty(CounterClockwiseId))
                {
                    if (Context == null)
                    {
                        throw new InvalidOperationException("The context is not set for this entity.");
                    }
                    _counterClockwise = Context.Cells.Find(CounterClockwiseId);
                    Context.Attach(_counterClockwise);
                }
                return _counterClockwise;
            }
            set
            {
                if (_counterClockwise != value)
                {
                    if (_counterClockwise != null)
                    {
                        if (_counterClockwise. != null)
                        {
                            _counterClockwise..Remove(this as Cell);
                        }
                    }

                    _counterClockwise = value;
                    CounterClockwiseId = _counterClockwise?.CellId ?? string.Empty;

                    if (_counterClockwise != null)
                    {
                        if (_counterClockwise. != null)
                        {
                            _counterClockwise..Add(this as Cell);
                        }
                    }
                }
            }
        }

        public string CounterClockwiseId { get; set; }

        private Cell _flip;
        public virtual Cell Flip
        {
            get
            {
                if (_flip == null && !string.IsNullOrEmpty(FlipId))
                {
                    if (Context == null)
                    {
                        throw new InvalidOperationException("The context is not set for this entity.");
                    }
                    _flip = Context.Cells.Find(FlipId);
                    Context.Attach(_flip);
                }
                return _flip;
            }
            set
            {
                if (_flip != value)
                {
                    if (_flip != null)
                    {
                        if (_flip. != null)
                        {
                            _flip..Remove(this as Cell);
                        }
                    }

                    _flip = value;
                    FlipId = _flip?.CellId ?? string.Empty;

                    if (_flip != null)
                    {
                        if (_flip. != null)
                        {
                            _flip..Add(this as Cell);
                        }
                    }
                }
            }
        }

        public string FlipId { get; set; }

        private ObservableCollection<CellState> _cellStates;

        public ObservableCollection<CellState> CellStates
        {
            get
            {
                if (_cellStates == null)
                {
                    if (Context == null)
                    {
                        _cellStates = new ObservableCollection<CellState>();
                    }
                    else
                    {
                        var items = Context.CellStates.Where(x => x.CellId == this.CellId).ToList<CellState>();
                        _cellStates = new ObservableCollection<CellState>(items);
                        Context.AttachRange(items);
                    }
                    _cellStates.CollectionChanged += CellStates_CollectionChanged;
                }
                return _cellStates;
            }
            private set
            {
                if (_cellStates != null)
                {
                    _cellStates.CollectionChanged -= CellStates_CollectionChanged;
                }
                _cellStates = value;
                if (_cellStates != null)
                {
                    _cellStates.CollectionChanged += CellStates_CollectionChanged;
                }
            }
        }

        private void CellStates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems != null)
            {
                foreach (var item in e.NewItems.Cast<CellState>())
                {
                    if (item.CurrentStateCells != this)
                    {
                        item.CurrentStateCells = this as CurrentStateCells;
                    }
                }
            }

            if (e?.OldItems != null)
            {
                foreach (var item in e.OldItems.Cast<CellState>())
                {
                    if (item.CurrentStateCells == this)
                    {
                        item.CurrentStateCells = null;
                    }
                }
            }
        }


        protected override void LazyLoadProperties()
        {
            var clockwise = this.Clockwise;
            var counterClockwise = this.CounterClockwise;
            var flip = this.Flip;
            var cellStates = this.CellStates;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}