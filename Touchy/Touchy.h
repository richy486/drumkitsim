// Touchy.h

#pragma once

#include "SynKit.h"

using namespace System;

namespace Touchy 
{
	public ref class TouchyFeely
	{
		// TODO: Add your methods for this class here.
	public:
		TouchyFeely();
		~TouchyFeely();
		!TouchyFeely();

		System::Boolean InitializeTouchPad();

	private:
		ISynAPI* m_pAPI;
		ISynDevice* m_pDevice;
	};
}