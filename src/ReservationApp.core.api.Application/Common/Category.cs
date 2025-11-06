using ReservationApp.core.api.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Application.Common
{
    public abstract class Category : SmartEnumeration<Category>
    {
        protected Category(int value, string description) : base(value, description)
        {
        }

        public static readonly Category Starter = new StarterCategory();
        public static readonly Category MainCourse = new MainCourseCategory();
        public static readonly Category Dessert = new DessertCategory();

        private sealed class StarterCategory : Category
        {
            public StarterCategory() : base(0, "Starter") { }
        }

        private sealed class MainCourseCategory : Category
        {
            public MainCourseCategory() : base(1, "Main Course") { }
        }

        private sealed class DessertCategory : Category
        {
            public DessertCategory() : base(2, "Dessert") { }
        }
    }
}
