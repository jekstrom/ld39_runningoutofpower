using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
	public abstract class Boss : MonoBehaviour
	{
		public virtual int Health { get; set; }
		public virtual Player Player { get; set; }
		public virtual Action OnDeath { get; set; }

		public virtual void TakeDamage(int damage) { }
	}
}
