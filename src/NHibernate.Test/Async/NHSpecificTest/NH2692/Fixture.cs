﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2692
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		[Test]
		public async Task QueryingParentWhichHasChildrenAsync()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				var result = await (session.Query<Parent>()
									.Where(x => x.ChildComponents.Any())
									.ToListAsync());

				Assert.That(result, Has.Count.EqualTo(1));
			}
		}

		[Test, KnownBug("NH-2692")]
		public async Task QueryingChildrenComponentsAsync()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				var result = await (session.Query<Parent>()
									.SelectMany(x => x.ChildComponents)
									.ToListAsync());

				Assert.That(result, Has.Count.EqualTo(1));
			}
		}

		[Test, KnownBug("NH-2692")]
		public async Task QueryingChildrenComponentsHqlAsync()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				var result = await (session.CreateQuery("select c from Parent as p join p.ChildComponents as c")
									.ListAsync<ChildComponent>());

				Assert.That(result, Has.Count.EqualTo(1));
			}
		}

		protected override void OnSetUp()
		{
			using (var session = OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var parent1 = new Parent();
				var child1 = new ChildComponent { Parent = parent1, SomeBool = true, SomeString = "something" };
				parent1.ChildComponents.Add(child1);

				var parent2 = new Parent();

				session.Save(parent1);
				session.Save(parent2);

				tx.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var tx = session.BeginTransaction())
			{
				session.Delete("from Parent");
				tx.Commit();
			}
		}
	}
}