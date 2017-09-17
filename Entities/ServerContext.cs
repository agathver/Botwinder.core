﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Pomelo.EntityFrameworkCore.MySql;

using guid = System.UInt64;

namespace Botwinder.entities
{
	public class ServerContext: DbContext
	{
		public DbSet<ServerConfig> ServerConfigurations;
		public DbSet<ServerStats> ServerStats;
		public DbSet<ChannelConfig> Channels;
		public DbSet<RoleConfig> Roles;

		public DbSet<CommandOptions> CommandOptions;
		public DbSet<CommandChannelOptions> CommandChannelOptions;
		public DbSet<CustomCommand> CustomCommands;
		public DbSet<CustomAlias> CustomAliases;

		public DbSet<UserData> UserDatabase;

		public ServerContext(DbContextOptions<ServerContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CommandOptions>()
				.HasKey(p => new{p.ServerId, p.CommandId});

			modelBuilder.Entity<CommandChannelOptions>()
				.HasKey(p => new{p.ServerId, p.CommandId, p.ChannelId});

			modelBuilder.Entity<CustomCommand>()
				.HasKey(p => new{p.ServerId, p.CommandId});

			modelBuilder.Entity<CustomAlias>()
				.HasKey(p => new{p.ServerId, p.Alias});

			modelBuilder.Entity<UserData>()
				.HasKey(p => new{p.ServerId, p.UserId});

			modelBuilder.Entity<Username>()
				.HasOne(p => p.UserData)
				.WithMany(p => p.Usernames)
				.HasForeignKey(p => new{p.ServerId, p.UserId});

			modelBuilder.Entity<Nickname>()
				.HasOne(p => p.UserData)
				.WithMany(p => p.Nicknames)
				.HasForeignKey(p => new{p.ServerId, p.UserId});
		}

		public static ServerContext Create(string connectionString)
		{
			DbContextOptionsBuilder<ServerContext> optionsBuilder = new DbContextOptionsBuilder<ServerContext>();
			optionsBuilder.UseMySql(connectionString);

			ServerContext newContext = new ServerContext(optionsBuilder.Options);
			newContext.Database.EnsureCreated();
			return newContext;
		}
	}
}
