using System;

namespace BizHawk.Emulation.Common.Components.MC6800
{
	public partial class MC6800
	{
		private void IRQ_()
		{
			Regs[ADDR] = 0xFFF8;
			PopulateCURINSTR(IDLE,
							SET_E,
							DEC16, SP,
							WR_DEC_LO, SP, PC,
							WR_DEC_HI, SP, PC,
							WR_DEC_LO, SP, X,
							WR_DEC_HI, SP, X,
							WR_DEC_LO, SP, B,
							WR_DEC_LO, SP, A,
							WR, SP, CC,
							SET_I,
							RD_INC, ALU, ADDR,
							RD_INC, ALU2, ADDR,
							SET_ADDR, PC, ALU, ALU2);

			IRQS = 19;
		}

		private void NMI_()
		{
			Regs[ADDR] = 0xFFFC;
			PopulateCURINSTR(IDLE,
							SET_E,
							DEC16, SP,
							WR_DEC_LO, SP, PC,
							WR_DEC_HI, SP, PC,
							WR_DEC_LO, SP, X,
							WR_DEC_HI, SP, X,
							WR_DEC_LO, SP, B,
							WR_DEC_LO, SP, A,
							WR, SP, CC,
							SET_F_I,
							RD_INC, ALU, ADDR,
							RD_INC, ALU2, ADDR,
							SET_ADDR, PC, ALU, ALU2);

			IRQS = 19;
		}

		public bool NMIPending;
		public bool IRQPending;
		public bool IN_SYNC;

		public Action IRQCallback = delegate () { };
		public Action NMICallback = delegate () { };

		private void ResetInterrupts()
		{
			IN_SYNC = false;
		}
	}
}