using ErrorOr;
using OpenMediator;
using ReservationApp.core.api.Application.Common;
using ReservationApp.core.api.Application.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReservationApp.core.api.Application.Reservation.Commands.ConfirmAppointment
{
    public class ManageStatusCommand : ICommand<ErrorOr<PostResponse>>
    {
        public int AppointmentId { get; set; }
        public ReservationStatus Status { get; set; }

        public ManageStatusCommand(int appointmentId, ReservationStatus status)
        {
            AppointmentId = appointmentId;
            Status = status;
        }
    }
}
