﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using snsrCollector.core;

namespace snsrCollector
{
    public partial class snsrContext : DbContext
    {
        public snsrContext()
        {
        }

        public snsrContext(DbContextOptions<snsrContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<DeviceLogical> DeviceLogical { get; set; }
        public virtual DbSet<DeviceObject> DeviceObject { get; set; }
        public virtual DbSet<DeviceObjectValue> DeviceObjectValue { get; set; }
        public virtual DbSet<DeviceProfile> DeviceProfile { get; set; }
        public virtual DbSet<Model> Model { get; set; }
        public virtual DbSet<ModelLogicalDevice> ModelLogicalDevice { get; set; }
        public virtual DbSet<ModelLogicalDeviceObject> ModelLogicalDeviceObject { get; set; }
        public virtual DbSet<ModelLogicalType> ModelLogicalType { get; set; }
        public virtual DbSet<ModelProfile> ModelProfile { get; set; }
        public virtual DbSet<ModelType> ModelType { get; set; }
        public virtual DbSet<Network> Network { get; set; }
        public virtual DbSet<ObjectDict> ObjectDict { get; set; }
        public virtual DbSet<ObjectTypeDict> ObjectTypeDict { get; set; }
        public virtual DbSet<ProfileType> ProfileType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                var config = ApplicationCore.GetInstance().GetAppConfig();

                var dbHost = config.GetKeyValueOr("DbHost", "localhost");
                var dbName = config.GetKeyValueOr("DbName", "snsr");
                var dbUser = config.GetKeyValueOr("DbUser", "postgres");
                var dbPassword = config.GetKeyValueOr("DbPassword", "masterkey");

                optionsBuilder.UseNpgsql($"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPassword}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("device_pkey");

                entity.ToTable("device");

                entity.HasComment("Прибор - экземпляр модели, представляющий и отображающий настоящий где-либо расположенный прибор в реальной жизни");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .HasColumnType("character varying");

                entity.Property(e => e.DeviceType).HasColumnName("Device_type");

                entity.Property(e => e.MainLogicalDevice)
                    .HasColumnName("Main_logical_device")
                    .HasColumnType("character varying");

                entity.Property(e => e.ModelFkey)
                    .IsRequired()
                    .HasColumnName("Model_fkey")
                    .HasColumnType("character varying");

                entity.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasColumnName("Serial_number")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.DeviceTypeNavigation)
                    .WithMany(p => p.Device)
                    .HasForeignKey(d => d.DeviceType)
                    .HasConstraintName("device_Device_type_fkey");

                entity.HasOne(d => d.MainLogicalDeviceNavigation)
                    .WithMany(p => p.Device)
                    .HasForeignKey(d => d.MainLogicalDevice)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("device_Main_logical_device_fkey");

                entity.HasOne(d => d.ModelFkeyNavigation)
                    .WithMany(p => p.Device)
                    .HasForeignKey(d => d.ModelFkey)
                    .HasConstraintName("device_Model_fkey_fkey");
            });

            modelBuilder.Entity<DeviceLogical>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("device_logical_pkey");

                entity.ToTable("device_logical");

                entity.HasComment("device_logical - связь логического прибора созданного экземпляра с тем прибором, что есть в модели, по которой создано отображение физического прибора.");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .HasColumnType("character varying");

                entity.Property(e => e.DeviceFkey)
                    .IsRequired()
                    .HasColumnName("Device_fkey")
                    .HasColumnType("character varying");

                entity.Property(e => e.ModelLogicalDevice)
                    .IsRequired()
                    .HasColumnName("Model_logical_device")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.DeviceFkeyNavigation)
                    .WithMany(p => p.DeviceLogical)
                    .HasForeignKey(d => d.DeviceFkey)
                    .HasConstraintName("device_logical_Device_fkey_fkey");

                entity.HasOne(d => d.ModelLogicalDeviceNavigation)
                    .WithMany(p => p.DeviceLogical)
                    .HasForeignKey(d => d.ModelLogicalDevice)
                    .HasConstraintName("device_logical_Model_logical_device_fkey");
            });

            modelBuilder.Entity<DeviceObject>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("device_object_pkey");

                entity.ToTable("device_object");

                entity.HasComment("device_object - связь объекта созданного экземпляра прибора с тем, что в модели");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .HasColumnType("character varying");

                entity.Property(e => e.DeviceLdFkey)
                    .IsRequired()
                    .HasColumnName("Device_ld_fkey")
                    .HasColumnType("character varying");

                entity.Property(e => e.DeviceProfileFkey)
                    .HasColumnName("Device_profile_fkey")
                    .HasColumnType("character varying");

                entity.Property(e => e.ModelObjectFkey)
                    .IsRequired()
                    .HasColumnName("Model_object_fkey")
                    .HasColumnType("character varying");

                entity.Property(e => e.ObjectDictId).HasColumnName("Object_dict_id");

                entity.Property(e => e.StartValue)
                    .HasColumnName("Start_value")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.DeviceLdFkeyNavigation)
                    .WithMany(p => p.DeviceObject)
                    .HasForeignKey(d => d.DeviceLdFkey)
                    .HasConstraintName("device_object_Device_ld_fkey_fkey");

                entity.HasOne(d => d.DeviceProfileFkeyNavigation)
                    .WithMany(p => p.DeviceObject)
                    .HasForeignKey(d => d.DeviceProfileFkey)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("device_object_Device_profile_fkey_fkey");

                entity.HasOne(d => d.ModelObjectFkeyNavigation)
                    .WithMany(p => p.DeviceObject)
                    .HasForeignKey(d => d.ModelObjectFkey)
                    .HasConstraintName("device_object_Model_object_fkey_fkey");

                entity.HasOne(d => d.ObjectDict)
                    .WithMany(p => p.DeviceObject)
                    .HasForeignKey(d => d.ObjectDictId)
                    .HasConstraintName("device_object_Object_dict_id_fkey");
            });

            modelBuilder.Entity<DeviceObjectValue>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("device_object_value_pkey");

                entity.ToTable("device_object_value");

                entity.HasComment("device_object_value - значение объекта в устройстве. Например, значение температуры.");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .HasColumnType("character varying");

                entity.Property(e => e.DeviceObjectFkey)
                    .IsRequired()
                    .HasColumnName("Device_object_fkey")
                    .HasColumnType("character varying");

                entity.Property(e => e.ObjectValue)
                    .IsRequired()
                    .HasColumnName("Object_value")
                    .HasColumnType("character varying");

                entity.Property(e => e.ReceiveTime).HasColumnName("Receive_time");

                entity.HasOne(d => d.DeviceObjectFkeyNavigation)
                    .WithMany(p => p.DeviceObjectValue)
                    .HasForeignKey(d => d.DeviceObjectFkey)
                    .HasConstraintName("device_object_value_Device_object_fkey_fkey");
            });

            modelBuilder.Entity<DeviceProfile>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("device_profile_pkey");

                entity.ToTable("device_profile");

                entity.HasComment("device_profile - связь профиля созданного экземпляра модели прибора.");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .HasColumnType("character varying");

                entity.Property(e => e.DeviceLdFkey)
                    .IsRequired()
                    .HasColumnName("Device_ld_fkey")
                    .HasColumnType("character varying");

                entity.Property(e => e.ModelProfileFkey)
                    .IsRequired()
                    .HasColumnName("Model_profile_fkey")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.DeviceLdFkeyNavigation)
                    .WithMany(p => p.DeviceProfile)
                    .HasForeignKey(d => d.DeviceLdFkey)
                    .HasConstraintName("device_profile_Device_ld_fkey_fkey");

                entity.HasOne(d => d.ModelProfileFkeyNavigation)
                    .WithMany(p => p.DeviceProfile)
                    .HasForeignKey(d => d.ModelProfileFkey)
                    .HasConstraintName("device_profile_Model_profile_fkey_fkey");
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("model_pkey");

                entity.ToTable("model");

                entity.HasComment("Модель прибора - описание, чертёж прибора. Ближайшее сравнение - классы в ООП. По их лекалу будут создаваться экземпляры, соответствующие физическим приборам, расположенным в реальном мире.");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .HasColumnType("character varying");

                entity.Property(e => e.ModelTypeFkey).HasColumnName("Model_type_fkey");

                entity.HasOne(d => d.ModelTypeFkeyNavigation)
                    .WithMany(p => p.Model)
                    .HasForeignKey(d => d.ModelTypeFkey)
                    .HasConstraintName("model_Model_type_fkey_fkey");
            });

            modelBuilder.Entity<ModelLogicalDevice>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("model_logical_device_pkey");

                entity.ToTable("model_logical_device");

                entity.HasComment("Логический прибор - слой в модели, который содержит в себе профили и объекты. Необходим для идентификации объектов, к примеру, если в приборе есть два микроконтроллера, измеряющие температуру, для идентификации используются разные логические приборы. Будет создано 2 логических прибора, в каждом по объекту \"Температура\"");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .HasColumnType("character varying");

                entity.Property(e => e.LdType).HasColumnName("Ld_type");

                entity.Property(e => e.ModelFkey)
                    .IsRequired()
                    .HasColumnName("Model_fkey")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.LdTypeNavigation)
                    .WithMany(p => p.ModelLogicalDevice)
                    .HasForeignKey(d => d.LdType)
                    .HasConstraintName("model_logical_device_Ld_type_fkey");

                entity.HasOne(d => d.ModelFkeyNavigation)
                    .WithMany(p => p.ModelLogicalDevice)
                    .HasForeignKey(d => d.ModelFkey)
                    .HasConstraintName("model_logical_device_Model_fkey_fkey");
            });

            modelBuilder.Entity<ModelLogicalDeviceObject>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("model_logical_device_object_pkey");

                entity.ToTable("model_logical_device_object");

                entity.HasComment("Объекты логического прибора - ссылки, связывающие профили и логические приборы с словарём объектов");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .HasColumnType("character varying");

                entity.Property(e => e.ModelLdFkey)
                    .IsRequired()
                    .HasColumnName("Model_ld_fkey")
                    .HasColumnType("character varying");

                entity.Property(e => e.ModelProfileFkey)
                    .HasColumnName("Model_profile_fkey")
                    .HasColumnType("character varying");

                entity.Property(e => e.ObjectId).HasColumnName("Object_id");

                entity.HasOne(d => d.ModelLdFkeyNavigation)
                    .WithMany(p => p.ModelLogicalDeviceObject)
                    .HasForeignKey(d => d.ModelLdFkey)
                    .HasConstraintName("model_logical_device_object_Model_ld_fkey_fkey");

                entity.HasOne(d => d.ModelProfileFkeyNavigation)
                    .WithMany(p => p.ModelLogicalDeviceObject)
                    .HasForeignKey(d => d.ModelProfileFkey)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("model_logical_device_object_Model_profile_fkey_fkey");

                entity.HasOne(d => d.Object)
                    .WithMany(p => p.ModelLogicalDeviceObject)
                    .HasForeignKey(d => d.ObjectId)
                    .HasConstraintName("model_logical_device_object_Object_id_fkey");
            });

            modelBuilder.Entity<ModelLogicalType>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("model_logical_type_pkey");

                entity.ToTable("model_logical_type");

                entity.HasComment("Тип логического прибора - словарь с перечислением типов логических приборов.");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .ValueGeneratedNever();

                entity.Property(e => e.LdTypeName)
                    .IsRequired()
                    .HasColumnName("Ld_type_name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<ModelProfile>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("model_profile_pkey");

                entity.ToTable("model_profile");

                entity.HasComment("Профиль содержит информацию для коммуникации с прибором.");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .HasColumnType("character varying");

                entity.Property(e => e.ModelLdFkey)
                    .IsRequired()
                    .HasColumnName("Model_ld_fkey")
                    .HasColumnType("character varying");

                entity.Property(e => e.ProfileType).HasColumnName("Profile_type");

                entity.HasOne(d => d.ModelLdFkeyNavigation)
                    .WithMany(p => p.ModelProfile)
                    .HasForeignKey(d => d.ModelLdFkey)
                    .HasConstraintName("model_profile_Model_ld_fkey_fkey");

                entity.HasOne(d => d.ProfileTypeNavigation)
                    .WithMany(p => p.ModelProfile)
                    .HasForeignKey(d => d.ProfileType)
                    .HasConstraintName("model_profile_Profile_type_fkey");
            });

            modelBuilder.Entity<ModelType>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("model_type_pkey");

                entity.ToTable("model_type");

                entity.HasComment("Тип модели - словарь с перечислением типов.");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .ValueGeneratedNever();

                entity.Property(e => e.ModelTypeName)
                    .IsRequired()
                    .HasColumnName("Model_type_name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Network>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("network_pkey");

                entity.ToTable("network");

                entity.HasComment("network - сеть, граф, для того, чтобы в системе понимать связь между приборами.");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .ValueGeneratedNever();

                entity.Property(e => e.LeftDeviceId)
                    .IsRequired()
                    .HasColumnName("Left_device_id")
                    .HasColumnType("character varying");

                entity.Property(e => e.RightDeviceId)
                    .IsRequired()
                    .HasColumnName("Right_device_id")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.LeftDevice)
                    .WithMany(p => p.NetworkLeftDevice)
                    .HasForeignKey(d => d.LeftDeviceId)
                    .HasConstraintName("network_Left_device_id_fkey");

                entity.HasOne(d => d.RightDevice)
                    .WithMany(p => p.NetworkRightDevice)
                    .HasForeignKey(d => d.RightDeviceId)
                    .HasConstraintName("network_Right_device_id_fkey");
            });

            modelBuilder.Entity<ObjectDict>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("object_type_pkey");

                entity.ToTable("object_dict");

                entity.HasComment("Словарь объектов - перечисление объектов, которые могут содержаться в приборах. Например, температура, серийный номер, ip адрес.");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .ValueGeneratedNever();

                entity.Property(e => e.ObjectName)
                    .IsRequired()
                    .HasColumnName("Object_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.ObjectType).HasColumnName("Object_type");

                entity.HasOne(d => d.ObjectTypeNavigation)
                    .WithMany(p => p.ObjectDict)
                    .HasForeignKey(d => d.ObjectType)
                    .HasConstraintName("object_dict_Object_type_fkey");
            });

            modelBuilder.Entity<ObjectTypeDict>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("object_dict_pkey");

                entity.ToTable("object_type_dict");

                entity.HasComment(@"Словарь типов объектов - перечисление типов (Число, коммуникационный объект, строка, т.д.)
");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .ValueGeneratedNever();

                entity.Property(e => e.ObjectTypeName)
                    .IsRequired()
                    .HasColumnName("Object_type_name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<ProfileType>(entity =>
            {
                entity.HasKey(e => e.IdKey)
                    .HasName("profile_type_pkey");

                entity.ToTable("profile_type");

                entity.Property(e => e.IdKey)
                    .HasColumnName("ID_Key")
                    .ValueGeneratedNever();

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasColumnName("Type_name")
                    .HasColumnType("character varying");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
