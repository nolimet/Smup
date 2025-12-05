using UpgradeSystem;
using Util.Saving;

namespace Managers
{
	public static class SaveDataManager
	{
		private static UpgradeData _upgradeData;
		public static UpgradeData Upgrades
		{
			get
			{
				if (_upgradeData is not null || Serialization.TryLoadBinary(out _upgradeData))
				{
					return _upgradeData;
				}

				_upgradeData = new UpgradeData();
				Serialization.SaveBinary(_upgradeData);

				return _upgradeData;
			}
		}

		public static void SaveAll()
		{
			if (_upgradeData is not null)
			{
				Serialization.SaveBinary(_upgradeData);
			}
		}
	}
}
