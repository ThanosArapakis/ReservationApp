using ReservationApp.core.api.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Common
{
    public abstract class ReservationStatus : SmartEnumeration<ReservationStatus>
    {
        protected ReservationStatus(int value, string description) : base(value, description)
        {
        }

        public static readonly ReservationStatus Pending = new PendingStatus();
        public static readonly ReservationStatus Confirmed = new ConfirmedStatus();
        public static readonly ReservationStatus Cancelled = new CancelledStatus();
        public static readonly ReservationStatus Completed = new CompletedStatus();
        public static readonly ReservationStatus NoShow = new NoShowStatus();
        public static readonly ReservationStatus Rescheduled = new RescheduledStatus();

        private sealed class PendingStatus : ReservationStatus
        {
            public PendingStatus() : base(0, "Αρχικοποίηση") { }
        }

        private sealed class ConfirmedStatus : ReservationStatus
        {
            public ConfirmedStatus() : base(1, "Επιβεβαιωμένο") { }
        }

        private sealed class CancelledStatus : ReservationStatus
        {
            public CancelledStatus() : base(2, "Ακυρωμένο") { }
        }

        private sealed class CompletedStatus : ReservationStatus
        {
            public CompletedStatus() : base(3, "Ολοκληρώθηκε") { }
        }

        private sealed class NoShowStatus : ReservationStatus
        {
            public NoShowStatus() : base(4, "Δεν εμφανίστηκε") { }
        }

        private sealed class RescheduledStatus : ReservationStatus
        {
            public RescheduledStatus() : base(5, "Μετατέθηκε") { }
        }
}
}
