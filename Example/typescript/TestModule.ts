import { Stream } from '../Stream';
import * as AllType from './AllType';
/**************************************** 数据配置表 *****************************************/
/** Test表数据类 */
export class TestCfg extends Stream {
	static readonly relative = 'testmodule/test.da';
	static readonly refence = 'TestModuleTestCfg';
	private _cfgs: Test[] = [];
	constructor(rootDir: string) {
		super(rootDir + TestCfg.relative);
	}
	Get(id: number): Test | undefined {
		if (this.hasLoaded) {
			return this._cfgs[id];
		}
		else {
			this.LoadConfig();
			return this._cfgs[id];
		}
	}
	protected ParseConfig() {
		this._cfgs = this.GetList('TestModuleTest');
	}
	[Symbol.iterator]() { return this._cfgs.values(); }
}
export let _CFG_CLASS_ = [TestCfg];

/**************************************** 数据结构定义 *****************************************/

/**  */
export class TClass {
	/** Var1 */
	readonly Var1: string;
	/** Var2 */
	readonly Var2: number;
	constructor(stream: any) {
		this.Var1 = stream.GetString();
		this.Var2 = stream.GetFloat();
	}
}
Object.defineProperty(Stream.prototype, 'GetTestModuleTClassMaker', {
	value: (stream: any) => stream[`Get${stream.GetString()}`].bind(stream),
	writable: false,
});
Object.defineProperty(Stream.prototype, 'GetTestModuleTClass', {
	value: (stream: any) => new TClass(stream),
	writable: false,
});
/**  */
export class TM1 extends TClass {
	/** 继承1 */
	readonly V3: number;
	constructor(stream: any) {
		super(stream);
		this.V3 = stream.GetLong();
	}
}
Object.defineProperty(Stream.prototype, 'GetTestModuleTM1', {
	value: (stream: any) => new TM1(stream),
	writable: false,
});
/**  */
export class TM2 extends TClass {
	/** 继承2 */
	readonly V4: boolean;
	constructor(stream: any) {
		super(stream);
		this.V4 = stream.GetBool();
	}
}
Object.defineProperty(Stream.prototype, 'GetTestModuleTM2', {
	value: (stream: any) => new TM2(stream),
	writable: false,
});
/**  */
export class Test {
	/** 继承2 */
	readonly TID: number;
	/** 继承2 */
	readonly Name: string;
	/** 外部模块类型 */
	readonly Card: AllType.CardElement;
	constructor(stream: any) {
		this.TID = stream.GetInt();
		this.Name = stream.GetString();
		this.Card = <AllType.CardElement> stream.GetInt();
	}
}
Object.defineProperty(Stream.prototype, 'GetTestModuleTest', {
	value: (stream: any) => new Test(stream),
	writable: false,
});

/**************************************** 声明与导出 *****************************************/
declare module '../CfgManager' {
	interface CfgManager {
		get TestModuleTestCfg(): TestCfg;
	}
}

declare module '../Stream' {
	interface StreamBase {
		GetTClass(): TClass;
		GetTM1(): TM1;
		GetTM2(): TM2;
	}
}

