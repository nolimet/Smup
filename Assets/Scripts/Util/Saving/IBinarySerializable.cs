namespace Util.Saving
{
	public interface IBinarySerializable
	{
		string FileName { get; }

		byte[] GetBytes();
		void ApplyBytes(byte[] bytes);
	}
}
