using System.Data.Entity.Migrations;

namespace Kepler.Common.Migrations
{
    public  class CreateIndex : DbMigration
    {
        public override void Up()
        {
            // InfoObject "Id"  "Name"  "Status"



            CreateIndex("Car", new string[] { "BrandId", "RegistrationNumber" },
                true, "IX_Car_BrandId");


        }
    }
}