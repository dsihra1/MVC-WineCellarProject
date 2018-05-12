namespace username_Wines.DAL.WineMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using username_Wines.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<username_Wines.DAL.wineEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DAL\WineMigrations";
        }
        /// Wrapper for SaveChanges adding the Validation Messages to the generated exception
        /// </summary>
        /// <param name="context">The context.</param>
        /// Just replace calls to context.SaveChanges() with SaveChanges(context) in your seed method.
        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
            catch (Exception e)
            {
                throw new Exception(
                     "Seed Failed - errors follow:\n" +
                     e.InnerException.InnerException.Message.ToString(), e
                 ); // Add the original exception as the innerException
            }
        }
        protected override void Seed(username_Wines.DAL.wineEntities context)
        {
            var Wine_Types = new List<Wine_Type>
            {
                new Wine_Type {  wtName="Red"},
                new Wine_Type { wtName="White"}  ,
                new Wine_Type { wtName = "Port" }  
            };
            Wine_Types.ForEach(d => context.Wine_Types.AddOrUpdate(n => n.wtName, d));
            SaveChanges(context);

            var Wines = new List<Wine>
            {
                new Wine {  wineName="Riesling", wineYear="2007", winePrice=19.99m, wineHarvest=DateTime.Parse("2007-04-23"),
                    Wine_TypeID =(context.Wine_Types.Where(d=>d.wtName=="White").SingleOrDefault().ID) },
                new Wine {  wineName="Shariz", wineYear="2008", winePrice=29.99m, wineHarvest=DateTime.Parse("2008-07-23"),
                    Wine_TypeID =(context.Wine_Types.Where(d=>d.wtName=="Red").SingleOrDefault().ID) }
            };
            Wines.ForEach(p => context.Wines.AddOrUpdate(o => o.wineName, p));
            SaveChanges(context);
        }
    }
}
