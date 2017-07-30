using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
	public interface IWeapon
	{
		int Damage { get; }
		bool HoldToShoot { get; }
		bool Shoot();
		void StopShooting();
	}
}
