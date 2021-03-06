﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using Iesi.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Event;
using NHibernate.Mapping;
using Environment = System.Environment;

namespace Studyzy.LearnEnglishBySubtitle.Import.Hujiang
{
    public class NHibernateHelper
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibernateHelper(string fileName)
        {
            _sessionFactory = GetSessionFactory(fileName);
        }

        public Configuration Configuration { get; set; }

        private ISessionFactory GetSessionFactory(string fileName)
        {
            Configuration = new Configuration();
            string path = Environment.CurrentDirectory + "\\"+fileName;
            Configuration.Properties["connection.connection_string"] = "Data Source=" + path + ";Version=3;";

            FluentConfiguration fluentConfiguration = Fluently.Configure(Configuration);
   
            InitMapping(fluentConfiguration);
            
            var sf= fluentConfiguration.BuildSessionFactory();
      
            return sf;
        }

        private void InitMapping(FluentConfiguration fluentConfiguration)
        {
            fluentConfiguration.Mappings(
                x =>
                    {
                        x.AutoMappings.Add(Generate());
#if DEBUG
                        x.AutoMappings.ExportTo(@"D:\Temp");
#endif
                    });

         
        }
        private AutoPersistenceModel Generate()
        {
            AutoPersistenceModel mappings = AutoMap.Assemblies(
                new AutoMapConfiguration(), new Assembly[] {this.GetType().Assembly});

            mappings.UseOverridesFromAssembly(this.GetType().Assembly);


            return mappings;
        }

        public ISession GetSession()
        {
            return _sessionFactory.OpenSession();
        }

    }
}